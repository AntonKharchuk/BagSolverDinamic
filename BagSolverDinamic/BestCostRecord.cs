
using System.Security.Principal;

namespace BagSolverDinamic
{
    public class BestCostRecord
    {
        public List<ResultVDECombination> SetOfSelectedVDEs { get; set; }

        public BestCostRecord()
        {
            SetOfSelectedVDEs = new List<ResultVDECombination>();
        }

        //Створюємо список конфігурації ВДЕ для цін W
        //
        //for each ціни currentCost, що currentCost<maxCost
        //
        //for each вде V зі списку ВДЕ
        //for each вде V зі списку ВДЕ
        //for each вде V зі списку ВДЕ
        //
        //
        //
        //
        //
        //
        //
        //Створюємо змінну leftCost = currentCost
        //Створюємо змінну currentLowerThanLeftCost, що поки = leftCost
        //
        //Створюємо змінну List зі списком ВДЕ на даній ітерації 
        //
        //Перевіряємо, чи можливо додати дане ВВЕ V до списку з допомогою функції CanAddInfo
        //
        //Якщо можемо, то додаємо V до List
        //
        //Встановлюємо значення leftCost = leftCost - V.p(ціна ВДЕ)
        //Встановлюємо значення currentLowerThanLeftCost = leftCost
        //
        //Поки currentLowerThanLeftCost>0
        //
        //Створюємо булеву змінну MatchRecordFound = false
        //
        //
        //Для кожної рекодної конфігурації R зі списку W для ціни currentLowerThanLeftCost
        //
        //Перевіряємо чи можливе додавання ВДЕ з R до List з допомогою функції CanAddInfoConfig
        //
        //Якщо додавання можливе, додаємо 
        //
        //Встановлюємо значення leftCost = leftCost - R.p (ціна за встановлення всіх ВДЕ з рекодної конфігурації)
        //Встановлюємо значення currentLowerThanLeftCost = leftCost
        //змінну MatchRecordFound = true
        //
        //Перевіряємо чи  MatchRecordFound = false
        //Якщо так, то задаємо занчення currentLowerThanLeftCost = currentLowerThanLeftCost-1
        //
        //Порівнюємо значення Потужності отриманої Конфігурації ВДЕ List з рекодними значеннями для данної ітерації
        //
        //Якщо значення Потужності більше за рекодр, встановлюємо новий рекорд
        //
        //Якщо значення Потужності збігається з рекодром, записуємо Конфігурацію ВДЕ, як альтернативний оптимум
        //
        //
        //
    }
}
