using System;
using System.Collections.Generic;
using VendingMachine;

namespace VendingMachine
{
public sealed class VendingMachine
 {
    private static VendingMachine instance;

    private readonly Dictionary<Item, int> machineItems;
    private readonly Stack<SaleRecord> saleRecords;
    private double machineBank;
    private List<Observer> machineObservers;

    private VendingMachine()
    {
        this.machineItems = new Dictionary<Item, int>();
        this.saleRecords = new Stack<SaleRecord>();
        this.machineBank = 0;
        this.machineObservers = new List<Observer>();
    }

    public static VendingMachine GetInstance()
    {
        if (instance == null)
            instance = new VendingMachine();
        return instance;
    }
    public double MachineBank
    {
        get
        {
            return this.machineBank;
        }
    }
    public Dictionary<Item, int> MachineItems
    {
        get
        {
            return this.machineItems;
        }
    }
    public Stack<SaleRecord> SaleRecords
    {
        get
        {
            return this.saleRecords;
        }
    }

    public List<SaleRecord> GetLastSaleRecords(int num)
    {
        List<SaleRecord> list;

        if (num <= 0)
            return null;

        list = new List<SaleRecord>();

        foreach (SaleRecord saleRecord in saleRecords)
        {
            list.Add(saleRecord);
            if (--num == 0)
                break;
        }

        return list;
    }
    public int GetItemStock(Item item)
    {
        if (machineItems.ContainsKey(item))
            return machineItems[item];

        return -1;
    }
    public int GetTotalMachineItems()
    {
        int totalItems = 0;
        foreach (Item item in machineItems.Keys)
            totalItems += machineItems[item];
        return totalItems;

    }

    public void RefillItems()
    {
        machineItems.Clear();
        this.machineItems.Add(ItemFactory.GetItem("Coca Cola 330"), 20);
        this.machineItems.Add(ItemFactory.GetItem("Coca Cola Zero 330"), 20);
        this.machineItems.Add(ItemFactory.GetItem("Fuze Tea 500"), 20);
        this.machineItems.Add(ItemFactory.GetItem("Pepsi Max 330"), 20);
        this.machineItems.Add(ItemFactory.GetItem("Pepsi Max 500"), 10);
        this.machineItems.Add(ItemFactory.GetItem("Evian 500"), 10);
        this.machineItems.Add(ItemFactory.GetItem("Lays Barbecue"), 3);
        this.machineItems.Add(ItemFactory.GetItem("Lays Sour Cream & Onion"), 1);

        NotifyAllObservers(new VendingMachineLog("Machine has been refilled."));
    }
    public void SellItem(Item item, double amountPaid)
    {
        machineBank += item.ItemPrice;
        machineItems[item]--;
        NotifyAllObservers(new VendingMachineLog("Item has been sold."));
    }
    public void AddSaleRecord(SaleRecord saleRecord)
    {
        this.saleRecords.Push(saleRecord);
        NotifyAllObservers(new VendingMachineLog("Sale Record Added"));
    }

    public void RegisterObserver(Observer observer)
    {
        this.machineObservers.Add(observer);
    }
    public void UnregisterObserver(Observer observer)
    {
        this.machineObservers.Remove(observer);
    }
    public void NotifyAllObservers(VendingMachineLog log)
    {
        foreach (Observer observer in this.machineObservers)
            observer.Update(log);
    }

    public override string ToString()
    {
        return string.Format("Unique Items: {0}, Total Items: {1}, Total Sales: {2}, Machine Bank: {3}",
            machineItems.Count, GetTotalMachineItems(), saleRecords.Count, machineBank.ToString("C"));
    }
}
}