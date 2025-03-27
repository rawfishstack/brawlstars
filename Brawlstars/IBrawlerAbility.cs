namespace brawlstars.Brawlstars;

public interface IBrawlerAbility {
    /// <summary>
    /// 是否在使用此能力。
    /// </summary>
    public bool Using { get; }
    
    /// <summary>
    /// 是否能使用此能力。
    /// </summary>
    public bool Usable { get; }
}