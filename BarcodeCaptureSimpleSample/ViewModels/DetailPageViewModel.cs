using BarcodeCaptureSimpleSample.Models;
using BarcodeCaptureSimpleSample.Services;

using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Data.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;

using System.Linq;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace BarcodeCaptureSimpleSample.ViewModels
{
    public class DetailPageViewModel : BaseViewModel, IBarcodeCaptureListener
    {

        private static IMessageService MessageService;
        private IViewfinder viewfinder;

        public Camera Camera { get; private set; } = ScannerModel.Instance.CurrentCamera;

        public DataCaptureContext DataCaptureContext { get; private set; } = ScannerModel.Instance.DataCaptureContext;

        public BarcodeCapture BarcodeCapture { get; private set; } = ScannerModel.Instance.BarcodeCapture;


        public IViewfinder Viewfinder
        {
            get { return this.viewfinder; }
            set
            {
                this.viewfinder = value;
                this.OnPropertyChanged(nameof(Viewfinder));
            }
        }

        public DetailPageViewModel()
        {
            this.InitializeScanner();
            this.SubscribeToAppMessages();
        }

        private void SubscribeToAppMessages()
        {
            MessagingCenter.Subscribe(this, App.MessageKeys.OnResume, callback: async (App app) => await this.OnResumeAsync());
            MessagingCenter.Subscribe(this, App.MessageKeys.OnSleep, callback: async (App app) => await this.OnSleep());
        }

        private void InitializeScanner()
        {
            // Register self as a listener to get informed whenever a new barcode got recognized.
            this.BarcodeCapture.AddListener(this);

            // Rectangular viewfinder with an embedded Scandit logo.
            // The rectangular viewfinder is displayed when the recognition is active and hidden when it is not.
            this.Viewfinder = new RectangularViewfinder(RectangularViewfinderStyle.Square, RectangularViewfinderLineStyle.Light);
        }

        public Task OnSleep()
        {
            return this.Camera?.SwitchToDesiredStateAsync(FrameSourceState.Off);
        }

        public async Task OnResumeAsync()
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await Permissions.RequestAsync<Permissions.Camera>();
                if (permissionStatus == PermissionStatus.Granted)
                {
                    await this.ResumeFrameSource();
                }
            }
            else
            {
                await this.ResumeFrameSource();
            }
        }

        private Task ResumeFrameSource()
        {
            // Switch camera on to start streaming frames.
            // The camera is started asynchronously and will take some time to completely turn on.
            return this.Camera?.SwitchToDesiredStateAsync(FrameSourceState.On);
        }

        public void OnBarcodeScanned(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        {
            if (!session.NewlyRecognizedBarcodes.Any())
            {
                return;
            }

            Barcode barcode = session.NewlyRecognizedBarcodes[0];

            // Stop recognizing barcodes for as long as we are displaying the result. There won't be any new results until
            // the capture mode is enabled again. Note that disabling the capture mode does not stop the camera, the camera
            // continues to stream frames until it is turned off.
            barcodeCapture.Enabled = false;

            // If you are not disabling barcode capture here and want to continue scanning, consider
            // setting the codeDuplicateFilter when creating the barcode capture settings to around 500
            // or even -1 if you do not want codes to be scanned more than once.

            // Get the human readable name of the symbology and assemble the result to be shown.
            SymbologyDescription description = new SymbologyDescription(barcode.Symbology);
            string result = "Scanned: " + barcode.Data + " (" + description.ReadableName + ")";



            if (MessageService == null)
            {
                MessageService = DependencyService.Get<IMessageService>();
            }

            MessageService.ShowAsync(result, () => barcodeCapture.Enabled = true);
        }


        public void OnObservationStarted(BarcodeCapture barcodeCapture)
        {
            
        }

        public void OnObservationStopped(BarcodeCapture barcodeCapture)
        {
            
        }

        public void OnSessionUpdated(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        {
            
        }
    }
}