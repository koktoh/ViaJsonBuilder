using Microsoft.AspNetCore.Components;

namespace ViaJsonBuilder.Components
{
    public abstract class ErrorComponentBase : ComponentBase
    {
        protected bool HasError { get; set; } = false;
        protected string Message { get; set; } = string.Empty;

        public void ResetError()
        {
            this.HasError = false;
            this.Message = string.Empty;
        }

        public void ShowError(string message)
        {
            this.Message = message;
            this.HasError = true;
        }
    }
}
