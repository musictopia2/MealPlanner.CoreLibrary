namespace MealPlanner.CoreLibrary.Components;
public class CustomButton : ButtonComponentBase
{
    public override string BackColor => cc1.Navy.ToWebColor;
    public override string TextColor => cc1.Aqua.ToWebColor;
    public override string DisabledColor => cc1.LightGray.ToWebColor; //hopefully no problem for disabled color.
    public override string FontSize => "40px";
}