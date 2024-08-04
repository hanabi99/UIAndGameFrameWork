using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LawofDemeter : MonoBehaviour
{
    /**
   * 雇员
   */
    public class Employee
    {
        private String name;

        public String getName()
        {
            return name;
        }

        public void setName(String name)
        {
            this.name = name;
        }
    }
    /**
     * 部门经理
     */
    public class Manager
    {
        public List<Employee> getEmployees(String department)
        {
            List<Employee> employees = new List<Employee>();
            for (int i = 0; i < 10; i++)
            {
                Employee employee = new Employee();
                // 雇员姓名
                employee.setName(department + i);
                employees.Add(employee);
            }
            return employees;
        }

        public void printEmployee(String name)
        {
            List<Employee> employees = this.getEmployees(name);
            foreach (Employee employee in employees)
            {
                print(employee.getName() + ";");
            }
        }
    }
    /**
     * 公司
     */
    public class Company
    {
        private Manager manager = new Manager();

        public void printEmployee(String name)
        {
            manager.printEmployee(name);
        }
    }

}
