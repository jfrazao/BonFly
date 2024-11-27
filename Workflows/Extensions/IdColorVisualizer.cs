using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using OpenCV.Net;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class IdColorVisualizer
{
    public int RadiusSize { get; set; } 
    public int NumberOfIds { get; set; } 
    Scalar ScalarHSV2BGR(double H, double S, double V) 
    {
        Mat rgb = new Mat(1,1, Depth.U8, 3);
        Mat hsv = new Mat(1,1, Depth.U8, 3);
        hsv[0] = Scalar.Rgb(V,S,H);
        CV.CvtColor(hsv, rgb, ColorConversion.Hsv2Bgr);
        return rgb[0];
    }
    public IObservable<IplImage> Process(IObservable<IplImage> source)
    {
        return source.Select(value => 
        {
            var output = value.Clone();
            //var image = value.Item1;
            //var output = new IplImage(image.Size,image.Depth, 3);
            //CV.CvtColor(image, output,ColorConversion.Gray2Bgr);
            
            int colorStep = 180/NumberOfIds;
            int coordStepY = output.Height/NumberOfIds;
            for (int i = 0; i < NumberOfIds; i++)
            {
                //CV.Circle(output, new Point(value.Item2[i]), RadiusSize, ScalarHSV2BGR(i*step,255,255), 2);
                CV.Circle(output, new Point(5,(i*coordStepY)+5), 4, ScalarHSV2BGR(i*colorStep,255,255), -1);
            }
            
            return output;

        });
    }
}
