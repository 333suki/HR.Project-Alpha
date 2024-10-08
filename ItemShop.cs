using System.Diagnostics;

class ItemShop
{
    public Dictionary<Item, int> Stock;
    private Player Player;

    //Instantiate ItemShop with a player field to access currency
    public ItemShop(Player player)
    {
        Player = player;
    }

    // Method used to make comparisons regarding the player's inventory and the shop's stock
    // Responsible for removing item from stock and adding it to the player's inventory, for a price
    public void PurchaseItem(Item item, int price)
    {
        Console.Clear();
        if (item is Consumable) {
            Console.WriteLine($"This product costs {price} gold coins. Purchase this item? (y/n)");
        } else {
            Console.WriteLine($"This product costs {price} gold coins and requires 5 silk and 5 bones. Purchase this item? (y/n)");
        }
        string purchaseInput = Console.ReadKey().KeyChar.ToString().ToLower();
        while (purchaseInput != "y" && purchaseInput != "n")
        {
            Console.WriteLine("Invalid input.");
            if (item is Consumable) {
                Console.WriteLine($"This product costs {price} gold coins. Purchase this item? (y/n)");
            } else {
                Console.WriteLine($"This product costs {price} gold coins and requires 5 silk and 5 bones. Purchase this item? (y/n)");
            }
            purchaseInput = Console.ReadKey().KeyChar.ToString().ToLower();
        }
        if (purchaseInput == "n")
        {
            Console.Clear();
            return;
        }

        if (Player.CurrentLocation.ID == 2)
        {
            bool checkWebs = false;
            bool checkBones = false;
            foreach (KeyValuePair<Item, int> playerItem in Player.Items)
            {
                if (playerItem.Key.ID == 8 && playerItem.Value >= 5)
                {
                    checkWebs = true;
                }

                if (playerItem.Key.ID == 13 && playerItem.Value >= 5)
                {
                    checkBones = true;
                }
            }

            if (!checkWebs || !checkBones) {
                Console.WriteLine("You don't have the required materials.");
                return;
            }

            if (checkWebs && checkBones) {
                Player.Items[new Item(13, "Skeleton Bone", "A bone dropped by a skeleton. It could be used to craft stronger weapons.")] -= 5;
                Player.Items[new Item(8, "Spider Silk", "Silk dropped by a spider. It looks quite sturdy, this could be used to craft new weapons.")] -= 5;
            }
        }

        if (Player.Coins >= price)
        {
            if (Stock[item] != 0)
            {
                // Checks if item already exists in player inventory
                // If it does, increases value by 1 rather than adding the same item separately
                if (Player.Items.ContainsKey(item))
                {
                    Player.Items[item] += 1;
                }
                else
                {
                    Player.Items[item] = 1;
                }
                Stock[item] -= 1;
                Player.Coins -= price;
                Console.Clear();
                Console.WriteLine($"\x1b[1m\x1b[93m{item.Name} has been added to your inventory.\x1b[0m");
                Console.WriteLine();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("This product is out of stock!");
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("You don't have enough funds.");
        }
    }

    //Method that gets called to create the alchemist's shop
    public void AlchemistCatalog()
    {
        Consumable potion = new Consumable(5, "Health Potion", "A refreshing potion that restores your health.", 15);
        Consumable bigPotion = new Consumable(6, "Greater Health Potion", "An improved potion that restores your health.", 30);

        if (Stock is null)
        {
            Stock = new Dictionary<Item, int>(){ {potion, 20}, {bigPotion, 10} };
        }

        Console.Clear();
        Console.WriteLine("Welcome, take a look around.");

        bool k = true;
        while (k)
        {
            Console.WriteLine($"\x1b[93mCoins: \x1b[0m{this.Player.Coins}");
            int itemNumber = 0;
            foreach (KeyValuePair<Item, int> kvp in Stock)
            {
                itemNumber += 1;
                if (kvp.Key.ID == 5) {
                    Console.WriteLine($"\n[{itemNumber}] {kvp.Key}: {kvp.Value} left \x1b[33m(3 coins)\x1b[0m");
                } else if (kvp.Key.ID == 6) {
                    Console.WriteLine($"\n[{itemNumber}] {kvp.Key}: {kvp.Value} left \x1b[33m(5 coins)\x1b[0m");
                } else {
                    Console.WriteLine($"\n[{itemNumber}] {kvp.Key}: {kvp.Value} left");
                }
            }

            Console.WriteLine("\n\x1b[36mPress enter to exit, type any number to purchase that item.\x1b[0m");

            string userPurchase = Console.ReadLine().ToUpper();

            //Will have to add more cases if you want to add more items to the shop
            switch (userPurchase)
            {
                case "1":
                    PurchaseItem(potion, 3);
                    break;
                case "2":
                    PurchaseItem(bigPotion, 5);
                    break;
                case "":
                case null:
                    k = false;
                    break;
            }
        }
    }

    //Method that gets called to create the town's shop
    public void TownCatalog()
    {
        Weapon greatSword = new Weapon(9, "Great sword", "A heavy, steel blade built to cut through armor with raw power.", 30, 7);
        Weapon quickfireBow = new Weapon(10, "Quickfire Bow", "A lightweight bow designed for rapid firing.", 27, 4);
        Weapon noviceWand = new Weapon(11, "Novice Wand", "A simple yet sturdy wand, designed for novice spellcasters to harness their first magical energies.", 28, 5);
        Weapon steelDagger = new Weapon(12, "Steel Dagger", "A sharp, compact dagger forged from durable steel, ideal for quick strikes and stealthy maneuvers.", 25, 5);

        if (Stock is null)
        {
            Stock = new Dictionary<Item, int>(){ {greatSword, 1}, {quickfireBow, 1}, {noviceWand, 1}, {steelDagger, 1} };
        }

        Console.Clear();
        Console.WriteLine("Welcome, take a look around. I'll need five bones and five silk to forge a new weapon.");

        bool k = true;
        while (k)
        {
            Console.WriteLine($"\x1b[93mCoins: \x1b[0m{this.Player.Coins}");
            int itemNumber = 0;
            foreach (KeyValuePair<Item, int> kvp in Stock)
            {
                itemNumber += 1;
                Console.WriteLine($"\n[{itemNumber}] {kvp.Key}: {kvp.Value} left \x1b[33m(10 coins)\x1b[0m");
            }
            if (this.Player.ClassName == "monk" && !this.Player.FistsUpgraded) {
                itemNumber++;
                Console.WriteLine($"\n[{itemNumber}] Upgrade your fist bandages.");
            }
            

            Console.WriteLine("\n\x1b[36mPress enter to exit, type any number to purchase that item.\x1b[0m");

            string userPurchase = Console.ReadLine().ToUpper();

            switch (userPurchase)
            {
                case "1":
                    if (this.Player.ClassName != "warrior") {
                        Console.Clear();
                        Console.WriteLine("You cannot purchase this weapon!");
                        break;
                    }
                    PurchaseItem(greatSword, 10);
                    break;
                case "2":
                    if (this.Player.ClassName != "archer") {
                        Console.Clear();
                        Console.WriteLine("You cannot purchase this weapon!");
                        break;
                    }
                    PurchaseItem(quickfireBow, 10);
                    break;
                case "3":
                    if (this.Player.ClassName != "sorcerer") {
                        Console.Clear();
                        Console.WriteLine("You cannot purchase this weapon!");
                        break;
                    }
                    PurchaseItem(noviceWand, 10);
                    break;
                case "4":
                    if (this.Player.ClassName != "rogue") {
                        Console.Clear();
                        Console.WriteLine("You cannot purchase this weapon!");
                        break;
                    }
                    PurchaseItem(steelDagger, 10);
                    break;
                case "5":
                    if (this.Player.ClassName != "monk" || this.Player.FistsUpgraded) {
                        Console.Clear();
                        break;
                    }
                    Console.Clear();
                    Console.WriteLine($"This product costs 10 gold coins and requires 5 silk and 5 bones. Purchase this item? (y/n)");
                    string purchaseInput = Console.ReadKey().KeyChar.ToString().ToLower();
                    while (purchaseInput != "y" && purchaseInput != "n") {
                        Console.Clear();
                        Console.WriteLine("Invalid input.");
                        Console.WriteLine($"This product costs 10 gold coins and requires 5 silk and 5 bones. Purchase this item? (y/n)");
                        purchaseInput = Console.ReadKey().KeyChar.ToString().ToLower();
                    }
                    if (purchaseInput == "n") {
                        Console.Clear();
                        break;
                    }

                    if (Player.CurrentLocation.ID == 2) {
                        bool checkWebs = false;
                        bool checkBones = false;
                        foreach (KeyValuePair<Item, int> playerItem in Player.Items) {
                            if (playerItem.Key.ID == 8 && playerItem.Value >= 5) {
                                checkWebs = true;
                            }

                            if (playerItem.Key.ID == 13 && playerItem.Value >= 5) {
                                checkBones = true;
                            }
                        }

                        if (!checkWebs || !checkBones) {
                            Console.WriteLine("You don't have the required materials.");
                            return;
                        }

                        if (checkWebs && checkBones) {
                            Player.Items[new Item(13, "Skeleton Bone", "A bone dropped by a skeleton. It could be used to craft stronger weapons.")] -= 5;
                            Player.Items[new Item(8, "Spider Silk", "Silk dropped by a spider. It looks quite sturdy, this could be used to craft new weapons.")] -= 5;
                        }
                    }

                    this.Player.FistsUpgraded = true;
                    this.Player.BareFistBonus = 6;
                    Console.Clear();
                    Console.WriteLine($"\x1b[1m\x1b[93mYour fists have been upgraded.\x1b[0m");
                    Console.WriteLine();
                    break;
                case "":
                case null:
                    k = false;
                    break;
                default:
                    Console.Clear();
                    break;
            }
        }
    }
}
