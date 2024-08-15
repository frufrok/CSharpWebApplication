namespace ProductStorageAPI.DTO
{
    public record StoragePayload(StorageDto storage);
    public record StorageInput(string name, string description);
}
