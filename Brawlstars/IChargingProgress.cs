namespace brawlstars.Brawlstars;

public interface IChargingProgress {
    /// <summary>
    /// 进度条，从0到100。
    /// </summary>
    public int Value { get; }
}