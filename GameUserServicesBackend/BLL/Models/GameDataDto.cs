using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    // Planted Trees DTOs
    public class PlantedTreeDto
    {
        [Required]
        public string ItemId { get; set; } = null!;
        
        [Required]
        public string Status { get; set; } = null!;
    }

    public class SavePlantedTreesRequest
    {
        [Required]
        public string UserId { get; set; } = null!;
        
        [Required]
        public List<PlantedTreeDto> Trees { get; set; } = new();
    }

    public class PlantedTreesResponse
    {
        public string UserId { get; set; } = null!;
        public List<PlantedTreeDto> Trees { get; set; } = new();
    }

    // Harvest Log DTOs
    public class HarvestDto
    {
        [Required]
        public string ItemId { get; set; } = null!;
        
        [Required]
        public string Status { get; set; } = null!;
    }

    public class SaveHarvestRequest
    {
        [Required]
        public string UserId { get; set; } = null!;
        
        [Required]
        public List<HarvestDto> Harvests { get; set; } = new();
    }

    public class HarvestLogResponse
    {
        public string UserId { get; set; } = null!;
        public List<HarvestDto> Harvests { get; set; } = new();
    }

    // Inventory DTOs
    public class InventoryItemDto
    {
        [Required]
        public string ItemId { get; set; } = null!;
        
        [Required]
        public int? Quantity { get; set; }
    }

    public class SaveInventoryRequest
    {
        [Required]
        public string UserId { get; set; } = null!;
        
        [Required]
        public List<InventoryItemDto> Items { get; set; } = new();
    }

    public class InventoryResponse
    {
        public string UserId { get; set; } = null!;
        public List<InventoryItemDto> Items { get; set; } = new();
    }

    // User Stats DTOs
    public class UserStatsDto
    {
        [Required]
        public string UserId { get; set; } = null!;
        
        public int Coin { get; set; }
        public int ExpPerLevel { get; set; }
        public int Level { get; set; }
        public string? MemberTypeId { get; set; }
        public string? Status { get; set; }
    }

    public class UpdateUserStatsRequest
    {
        [Required]
        public string UserId { get; set; } = null!;
        
        public int Coin { get; set; }
        public int ExpPerLevel { get; set; }
        public int Level { get; set; }
    }

    // Scene Data DTOs
    public class SceneDataDto
    {
        [Required]
        public string Status { get; set; } = null!;
        
        public DateTime DateSave { get; set; }
    }

    public class SaveSceneRequest
    {
        [Required]
        public string UserId { get; set; } = null!;
        
        [Required]
        public SceneDataDto SceneData { get; set; } = null!;
    }

    public class SceneResponse
    {
        public string UserId { get; set; } = null!;
        public SceneDataDto SceneData { get; set; } = null!;
    }

    // Scene Details DTOs
    public class SceneDetailDto
    {
        [Required]
        public string ItemId { get; set; } = null!;
        
        [Required]
        public string Name { get; set; } = null!;
    }

    public class SaveSceneDetailsRequest
    {
        [Required]
        public string UserId { get; set; } = null!;
        
        [Required]
        public List<SceneDetailDto> SceneDetails { get; set; } = new();
    }

    public class SceneDetailsResponse
    {
        public string UserId { get; set; } = null!;
        public List<SceneDetailDto> SceneDetails { get; set; } = new();
    }

    // Full Game Data Sync DTOs
    public class FullGameDataRequest
    {
        [Required]
        public string UserId { get; set; } = null!;
        
        public UserStatsDto? UserStats { get; set; }
        public List<InventoryItemDto>? Inventory { get; set; }
        public List<PlantedTreeDto>? PlantedTrees { get; set; }
        public List<HarvestDto>? HarvestLog { get; set; }
        public SceneDataDto? SceneData { get; set; }
    }

    public class FullGameDataResponse
    {
        public string UserId { get; set; } = null!;
        public UserStatsDto? UserStats { get; set; }
        public List<InventoryItemDto>? Inventory { get; set; }
        public List<PlantedTreeDto>? PlantedTrees { get; set; }
        public List<HarvestDto>? HarvestLog { get; set; }
        public SceneDataDto? SceneData { get; set; }
    }

    public class SyncResponse
    {
        public string Status { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime SyncTimestamp { get; set; }
        public List<string> SyncedSections { get; set; } = new();
    }
}
