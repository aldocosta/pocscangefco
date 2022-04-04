/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Threading;
using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Source.Unified;

namespace BarcodeCaptureSimpleSample.Models
{
    public class ScannerModel
    {
        // Enter your Scandit License key here.
        public const string SCANDIT_LICENSE_KEY = "AZ7xNA/cLl5WHbYPoRDq8VAEgAxmNGYcX3Qrq6FlCN38bzhOUQ46abgrPzoFPDySWy9PfyNko2bVPoKaIUdpenoo+jeqbn1HGCGXQ+9lqsN8VUaTw1I6rGRasdODZKmKNFjTdtJL0/YHWgo//UWkp9ZOkqqLW45whV8tNxh5u8TiVdEbDU+uWdFJ84mkYkafmCXfw40vwtBqUJuWiFwEO/sIRvwRVUXSRUcCbq9IvfFPZpNciUZd2qBbIIdDQuZUFC8q1kFlNLcGXQreZzfV7pNPhBTBTb4FmGmHIPB/5911cS92MFkNdCdgCVI/YO9fHWQPSLdjNaszbm+GG1YWpRtsLapNW+Zjnlqm5apUjSOIRuieTHvy5CMsojEJbiKAvWXHCoMsT32GVyvAIUeF979AeDqBQejgbXuLGN53PTOFTNC5pHgOCTtTL9kNeeDuwmE0+GBIW6lBBBjZUkAEjjdB65K7cZaKOQht+5dX3WkpRrTben1eApp6CjOjcibL7hneICU+hTcHCZbAPw9viH1Xj8QbvtjgCllAwfPsUG/zAS2wep+rs7XW9BCDzqEGFTJ1E49yQ/CRF6TMKDkhXGYV0k+NMpAB7dmcN7d84XQHURqjTBn2xZJTrCQpk0TaMupFfF6DEUMcDBmrSoHFizbtPybyM9qNkYpMm5DII0aUZcRUORIWPO+6kPWoHwS/UpQi5Y8/rCPC2wAItmdO6G7kNzDY8wKI3ZM9qhIbZBGqEitpm0KNnYYwdmgF8luWkZ8BjFja+y5SAytTVaFKYwll8lpvEtWD2Mbm0yOGjOzvVgumocsNLDgW4Az6jf+w7ZCXo0RWI0PYc97TIXQ1TbcoymyEMhiblFKJUe6PMahuvaRuEyabUoQSdI+eY4XwI5vaJKbOawqT4SSwvyJpLB1olZ6L0oiE2Qnt5SBpTCGNB9rYiqt/GE4NhJNET7KyZ2GJRCmUKRHuTeu8dIvUQnFkI8C/8LnCVRUbi0wuGoV7XKk7pJvCzEj9n/mgb1Ywm+wSwM3S3Y6LWmNvNhsVskXpeWgy/lHdtoR4LA0iuI7JZc+SZkd0q2Cj5bPylu1bEdenNdJzIEFRWW3yegSYYfcS8OpkwZ22Ozma3cIBteQovyIQ1t953jJ35X+CAaKjj23xH7u5diSzGGvP7q8pwVq7tSgFNEARLldFx5YqahTjCvXA9vCynn20yqgKhe4y";

        private static readonly Lazy<ScannerModel> instance = new Lazy<ScannerModel>(() => new ScannerModel(), LazyThreadSafetyMode.PublicationOnly);

        public static ScannerModel Instance => instance.Value;

        private ScannerModel()
        {
     
            this.CurrentCamera?.ApplySettingsAsync(this.CameraSettings);

            // Create data capture context using your license key and set the camera as the frame source.
            this.DataCaptureContext = DataCaptureContext.ForLicenseKey(SCANDIT_LICENSE_KEY);
            this.DataCaptureContext.SetFrameSourceAsync(this.CurrentCamera);

            // The barcode capturing process is configured through barcode capture settings
            // which are then applied to the barcode capture instance that manages barcode recognition.
            this.BarcodeCaptureSettings = BarcodeCaptureSettings.Create();

            // The settings instance initially has all types of barcodes (symbologies) disabled.
            // For the purpose of this sample we enable a very generous set of symbologies.
            // In your own app ensure that you only enable the symbologies that your app requires as
            // every additional enabled symbology has an impact on processing times.
            HashSet<Symbology> symbologies = new HashSet<Symbology>
            {
                Symbology.Ean13Upca,
                Symbology.Ean8,
                Symbology.Upce,
                Symbology.Qr,
                Symbology.DataMatrix,
                Symbology.Code39,
                Symbology.Code128,
                Symbology.InterleavedTwoOfFive
            };

            this.BarcodeCaptureSettings.EnableSymbologies(symbologies);
            this.BarcodeCapture = BarcodeCapture.Create(this.DataCaptureContext, this.BarcodeCaptureSettings);
        }

        #region DataCaptureContext
        public DataCaptureContext DataCaptureContext { get; private set; }
        #endregion

        #region CamerSettings
        public Camera CurrentCamera { get; private set; } = Camera.GetCamera(CameraPosition.WorldFacing);
        public CameraSettings CameraSettings { get; } = BarcodeCapture.RecommendedCameraSettings;
        #endregion

        #region BarcodeCapture
        public BarcodeCapture BarcodeCapture { get; private set; }
        public BarcodeCaptureSettings BarcodeCaptureSettings { get; private set; }
        #endregion
    }
}
