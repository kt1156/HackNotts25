using System;
using System.Diagnostics; 
using player.cs;

[Serializable]
public abstract class Item
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public int Cost { get; protected set; }
    public abstract void Use(Player user, GameManager gameManager);
}
public class MushroomStew : Item
{
    public MushroomStew()
    {
        Name = "Mushroom Stew";
        Description = "Adds +3 to your next dice roll.";
        Cost = 5;
    }

    public override void Use(Player user, GameManager gameManager)
    {
        Debug.WriteLine($"{user.PlayerName} used Mushroom Stew!");
        gameManager.ApplyRollBonus(user, 3);
        user.Inventory.Remove(this);
    }
}

public class DoubleDicePotion : Item
{
    public DoubleDicePotion()
    {
        Name = "Double Dice Potion";
        Description = "Roll two D10 dice for movement.";
        Cost = 8;
    }

    public override void Use(Player user, GameManager gameManager)
    {
        Debug.WriteLine($"{user.PlayerName} used Double Dice Potion!");
        gameManager.ApplyDoubleRoll(user);
        user.Inventory.Remove(this);
    }
}

public class ThiefsGlove : Item
{
    public ThiefsGlove()
    {
        Name = "Thief's Glove";
        Description = "Steal 10 Gold from a random opponent.";
        Cost = 10;
    }

    public override void Use(Player user, GameManager gameManager)
    {
        Debug.WriteLine($"{user.PlayerName} used Thief's Glove!");
        Player target = gameManager.GetRandomOpponent(user);
        if (target != null)
        {
            int stolenAmount = 10;
            if (target.GoldCount < 10)
            {
                stolenAmount = target.GoldCount;
            }

            target.RemoveGold(stolenAmount);
            user.AddGold(stolenAmount);
            Debug.WriteLine($"{user.PlayerName} stole {stolenAmount} gold from {target.PlayerName}!");
        }
        user.Inventory.Remove(this);
    }
}

public class CursedIdol : Item
{
    public CursedIdol()
    {
        Name = "Cursed Idol";
        Description = "Target an opponent. Their next dice roll is halved.";
        Cost = 7;
    }

    public override void Use(Player user, GameManager gameManager)
    {
        Debug.WriteLine($"{user.PlayerName} used CursedIdol! (Needs target selection UI)");
        // In a real game, this would pop up a UI to select a target.
        // For this example, we'll just pick a random one.
        Player target = gameManager.GetRandomOpponent(user);
        if (target != null)
        {
            gameManager.ApplyRollPenalty(target, 0.5f); // 0.5f for halved
            Debug.WriteLine($"{target.PlayerName}'s next roll will be halved!");
        }
        user.Inventory.Remove(this);
    }
}

public class SkeletonKey : Item
{
    public SkeletonKey()
    {
        Name = "SkeletonKey";
        Description = "Automatically opens a locked door on the board.";
        Cost = 6;
    }

    public override void Use(Player user, GameManager gameManager)
    {
        Debug.WriteLine($"{user.PlayerName} used SkeletonKey!");
        // This is a passive item. The movement system would check
        // if (player.Inventory.Contains(typeof(SkeletonKey)))
        // when it encounters a locked door.
        // For this example, we'll just log it.
        // A real implementation might set a flag: user.HasSkeletonKey = true;
        user.Inventory.Remove(this);
    }
}