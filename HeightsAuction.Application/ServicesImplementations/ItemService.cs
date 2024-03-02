using AutoMapper;
using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Application.Interfaces.Services;
using HeightsAuction.Common.Utilities;
using HeightsAuction.Domain;
using HeightsAuction.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HeightsAuction.Application.ServicesImplementations
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemService> _logger;
        public ItemService(IUnitOfWork unitOfWork,
            IMapper mapper, ILogger<ItemService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ApiResponse<CreateItemResponseDto>> CreateItemAsync(string userId, string roomId, CreateItemRequestDto requestDto)
        {
            try
            {
                var existingUser = await _unitOfWork.Users.GetByIdAsync(userId);
                if (existingUser == null)
                {
                    return ApiResponse<CreateItemResponseDto>.Failed(false, "User not found", 400, new List<string> { });
                }
                var existingRoom = await _unitOfWork.BiddingRooms.GetByIdAsync(roomId);
                if (existingRoom == null)
                {
                    return ApiResponse<CreateItemResponseDto>.Failed(false, "Bidding Room not found", 400, new List<string> { });
                }

                if (existingRoom.HasFinished)
                {
                    return ApiResponse<CreateItemResponseDto>.Failed(false, "Cannot add item to this BiddingRoom, BiddingRoom is closed", 400, new List<string> { });
                }

                var existingItemInRoom = await _unitOfWork.Items.FindAsync(i => i.BiddingRoomId == roomId);
                if (existingItemInRoom.Any())
                {
                    return ApiResponse<CreateItemResponseDto>.Failed(false, $"Cannot add item to this BiddingRoom, an Item ({existingItemInRoom.First().Id}) already exists in the BiddingRoom", 400, new List<string> { });
                }

                var existingItem = await _unitOfWork.Items.FindAsync(b => b.Name == requestDto.Name);
                if (existingItem.Any())
                {
                    return ApiResponse<CreateItemResponseDto>.Failed(false, "Item with same Name already exists", 400, new List<string> { });
                }

                var item = _mapper.Map<Item>(requestDto);
                item.UserId = userId;
                item.CreatedBy = userId;
                item.BiddingRoomId = roomId;
                await _unitOfWork.Items.AddAsync(item);
                await _unitOfWork.SaveChangesAsync();

                existingRoom.ItemId = item.Id;
                _unitOfWork.BiddingRooms.Update(existingRoom);
                await _unitOfWork.SaveChangesAsync();

                var response = _mapper.Map<CreateItemResponseDto>(item);
                response.ItemId = item.Id;
                response.ItemName = item.Name;
                item.UserId = userId;
                item.BiddingRoomId = roomId;
                return ApiResponse<CreateItemResponseDto>.Success(response, "Item created successfully.", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating Item: { ex}");
                return ApiResponse<CreateItemResponseDto>.Failed(false, $"Error occurred while creating Item", 500, new List<string> { });
            }

        }

        public async Task<ApiResponse<PageResult<IEnumerable<ItemResponseDto>>>> GetAllItemsAsync(int page, int perPage)
        {
            try
            {
                var allItems = await _unitOfWork.Items.GetAllAsync();
                if (allItems == null)
                {
                    return ApiResponse<PageResult<IEnumerable<ItemResponseDto>>>.Failed(false, "No items found", 400, new List<string> { });
                }

                var itemsWithRelatedEntities = new List<Item>();
                foreach (var item in allItems)
                {
                    itemsWithRelatedEntities.Add(await _unitOfWork.Items.IncludeRelatedEntities(item));
                }

                var pagedItems = await Pagination<Item>.GetPager(
                    itemsWithRelatedEntities,
                    perPage,
                    page,
                    item => item.Name,
                    item => item.Id.ToString());

                var pagedItemsDto = _mapper.Map<PageResult<IEnumerable<ItemResponseDto>>>(pagedItems);
                return ApiResponse<PageResult<IEnumerable<ItemResponseDto>>>.Success(pagedItemsDto, "Items found", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while retrieving items");
                return ApiResponse<PageResult<IEnumerable<ItemResponseDto>>>.Failed(false, "An error occured while retrieving items", 400, new List<string> { });

            }
        }
    }
}
