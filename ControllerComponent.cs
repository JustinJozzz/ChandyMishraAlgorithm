
[ViewComponent(Name = "Controller")]
public class ControllerComponent : ViewComponent
{
    public ControllerComponent(IAddressFormatter formatter)
    {
        this.formatter = formatter;
    }

    public async Task<IViewComponentResult> InvokeAsync(int employeeId)
    {
        var model = new AddressViewModel
        {
            EmployeeId = employeeId,
            Line1 = "Secret Location",
            Line2 = "London",
            Line3 = "UK"
        };
        model.FormattedValue =
              this.formatter.Format(model.Line1, model.Line2, model.Line3);
        return View(model);
    }
}