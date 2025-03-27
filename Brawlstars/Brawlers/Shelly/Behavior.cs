namespace brawlstars.Brawlstars.Brawlers.Shelly;

public class Behavior : BrawlerBehavior {
    public Behavior(BrawlerItem item) : base(item) {
    }

    protected override void BuildInfo(BrawlerInfo.BrawlerInfoBuilder builder) {
        builder
            .Hero(
                0,
                [],
                [[5, 600]],
                [],
                [[9, 640]],
                ["33%"],
                [25, 5, 25]
            )
            .Gadget([])
            .Gadget([5])
            .StarPower([2])
            .StarPower(["40%", 150, 15]);
    }

    protected override void InitHyperCharge() {
        base.InitHyperCharge();
        HyperChargeMultiplier[0] = 30;
    }

    protected override void InitAllNotImplementationMarked() {
        base.InitAllNotImplementationMarked();
        NotImplementedAll = true;
    }
}