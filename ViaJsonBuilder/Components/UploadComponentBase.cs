using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViaJsonBuilder.Extensions;

namespace ViaJsonBuilder.Components
{
    public abstract class UploadComponentBase : ErrorComponentBase
    {
        protected string Status { get; set; }

        [Parameter]
        public IEnumerable<string> TargetFiles { get; set; }

        protected async Task OnUpload(IFileListEntry[] files)
        {
            this.ResetError();

            var file = files.FirstOrDefault();

            if (file == null)
            {
                return;
            }

            if (!this.Validate(file))
            {
                this.ShowError($"{this.TargetFiles.Join(" / ")} をアップロードしてください。");
                return;
            }

            this.Status = "Loading...";

            try
            {
                await this.OnUploadTaskAsync(file);
            }
            catch
            {
                this.ShowError("ファイルのアップロードに失敗しました。");
            }

            this.Status = this.BuildDefaultStatusMessage();
        }

        protected override void OnInitialized()
        {
            this.Status = this.BuildDefaultStatusMessage();
            base.OnInitialized();
        }

        private string BuildDefaultStatusMessage()
        {
            return $"クリックまたはドラッグ&ドロップで {this.TargetFiles.Join(" / ")} をアップロード";
        }

        protected abstract bool Validate(IFileListEntry file);

        protected abstract Task OnUploadTaskAsync(IFileListEntry file);
    }
}
