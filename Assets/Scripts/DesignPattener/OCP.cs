using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OCP;
public abstract class AbstractChart
{
    public abstract void display();
}

public class OCP : MonoBehaviour
{
    public class PieChart : AbstractChart
    {
        public override void display()
        {

        }
    }
    public class BarChart : AbstractChart
    {
        public override void display()
        {

        }
    }

}
public class ChartDisPlay
{
    private AbstractChart chart;

    public void setChart(AbstractChart chart)
    {
        this.chart = chart;
    }
    public void display(String type)
    {
        chart.display();
    }
}

