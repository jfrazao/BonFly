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
    public IObservable<IplImage> Process(IObservable<Tuple<int,IplImage>> source)
    {
        return source.Select(value => 
        {
            var id = value.Item1;
 
            var output = value.Item2.Clone();

            
            int colorStep = 180/NumberOfIds;
            CV.Circle(output, new Point(5,5), 4, ScalarHSV2BGR(id*colorStep,255,255), -1);

            
            return output;

        });
    }
    public IObservable<IplImage> Process(IObservable<Tuple<IplImage,int>> source)
    {
        return source.Select(value => 
        {
            var id = value.Item2;
 
            var output = value.Item1.Clone();

            
            int colorStep = 180/NumberOfIds;
            CV.Circle(output, new Point(5,5), 4, ScalarHSV2BGR(id*colorStep,255,255), -1);

            
            return output;

        });
    }
}
