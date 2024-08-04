using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceSegregationPrinciple : MonoBehaviour
{
    
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}

public interface CustomerDataDisplay
{
    List<CustomerData> dataRead();
    void transformToXml();
    void createChart();
    void displayChart();
    void createReport();
    void displayReport();
}

public class CustomerData
{
}

public interface DataHandler
{
    List<CustomerData> dataRead();
}

public interface XMLTransformer
{
    void transformToXml();
}
public interface ChartHandler
{
   void createChart();
   void displayChart();
}
public interface ReportHandler
{
    void createReport();
    void displayReport();
}



