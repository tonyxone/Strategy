#region Using declarations
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Indicator;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Strategy;
#endregion

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    /// <summary>
    /// Enter the description of your strategy here
    /// </summary>
    [Description("Enter the description of your strategy here")]
    public class MACross : Strategy
    {
        #region Variables
        // Wizard generated variables
        private int myInput0 = 1; // Default setting for MyInput0
        // User defined variables (add any user defined variables below)
        #endregion

        /// <summary>
        /// This method is used to configure the strategy and is called once before any strategy method is called.
        /// </summary>
		
        private int barCount = 0;
		
		protected override void Initialize()
        {
            Add(SMA(5));
            Add(SMA(20));
			Add(SMA(30));
			Add(SMA(90));
			
			Add(PeriodType.Minute,60);
			
            CalculateOnBarClose = true;
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
			
			int sl1 = (int)(180 / Math.PI * (Math.Atan((SMA(5)[0] - (SMA(5)[1] + SMA(5)[2]) / 2) / 1.5 / TickSize)));

			this.barCount++;
			
			if ((ToTime(Time[0]) <= ToTime(19, 0, 0) && ToTime(Time[0]) >= ToTime(3, 0, 0)) 
				|| (ToTime(Time[0]) <= ToTime(13, 15, 0) && ToTime(Time[0]) >= ToTime(15, 30, 0)))
            {}
			
			
			//*****************************************************************************
			//buying
			
			bool shouldBuy = false;
			
			if(Closes[0] <= SMA(90)[0]){
				if(SMA(30)[0] <= SMA(90)[0]){
					if(CrossAbove(SMA(5), SMA(20), 3)){
					shouldBuy = true;
				}
				}
			}else{
				if(this.Rising(SMA(20)) && this.Rising(SMA(90))){
					if(CrossAbove(SMA(5), SMA(20), 3)){
						shouldBuy = true;
					}
				}
			}
			
				if(shouldBuy){
					EnterLong(1, "long1");
					Print(this.barCount);
				}
			
			
			
			//********************************************************************************
			//selling
			
			bool shouldSell = false;			
			
	
			
				if(CrossBelow(Closes[0],SMA(20),1)){
					shouldSell = true;
				}	
		
			if(shouldSell){
				ExitLong();
			}
					
		}
	
        #region Properties
        [Description("")]
        [GridCategory("Parameters")]
        public int MyInput0
        {
            get { return myInput0; }
            set { myInput0 = Math.Max(1, value); }
        }
        #endregion
    }
}
