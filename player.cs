namespace ConsoleApp1;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; // For Texture2D

/// <summary>
/// Defines the available character classes.
/// </summary>
public enum CharacterClass
{
    Knight,
    Wizard,
    Priest,
    King
}

/// <summary>
/// A data class to represent the player's visual pawn on the board.
/// This replaces Unity's GameObject.
/// </summary>
public class PlayerPawn
{
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }

    public PlayerPawn(Texture2D texture, Vector2 startPosition)
    {
        Texture = texture;
        Position = startPosition;
    }

    // A draw method for the UIManager or main Game class to call
    public void Draw(SpriteBatch spriteBatch)
    {
        if (Texture != null)
        {
            // Draw centered
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
                new Vector2(Texture.Width / 2f, Texture.Height / 2f),
                1f, SpriteEffects.None, 0f);
        }
    }
}


/// <summary>
/// Main class to store all data for a single player.
/// This would be instantiated 4 times at the start of the game.
/// </summary>
[Serializable]
public class Player
{
    public int PlayerID { get; private set; }
    public string PlayerName { get; set; }
    public CharacterClass CharacterClass { get; private set; }
    public int GoldCount { get; set; }
    public int ScepterCount { get; set; }
    public List<Item> Inventory { get; private set; }

    // This would be a reference to the player's current position on the board.
    // It's defined by 'nhi & oli'.
    public BoardSpace CurrentBoardSpace { get; set; }

    // This is the MonoGame equivalent of the player's 3D model
    public PlayerPawn PlayerPawn { get; set; }

    // --- Stats for End-Game Bonuses ---
    public int TotalGoldCollected { get; set; }
    public int MiniGamesWon { get; set; }
    public int EventSpacesLandedOn { get; set; }

    public Player(int id, CharacterClass characterClass, PlayerPawn pawn)
    {
        PlayerID = id;
        CharacterClass = characterClass;
        PlayerPawn = pawn;
        Inventory = new List<Item>();

        // Apply passive abilities from the game plan
        switch (characterClass)
        {
            case CharacterClass.Knight:
                GoldCount = 10;
                break;
            case CharacterClass.Wizard:
                GoldCount = 0; // Wizards get shop discount, not starting gold
                break;
            case CharacterClass.Priest:
                GoldCount = 0;
                break;
            case CharacterClass.King:
                GoldCount = 0; // King gets bonus from blue spaces
                break;
        }

        TotalGoldCollected = GoldCount;
    }

    /// <summary>
    /// Adds gold to the player, applying the King's bonus if applicable.
    /// </summary>
    public void AddGold(int amount, bool isFromBlueSpace = false)
    {
        if (isFromBlueSpace && CharacterClass == CharacterClass.King)
        {
            amount += 1; // King's passive ability
        }

        GoldCount += amount;
        TotalGoldCollected += amount;
    }

    /// <summary>
    /// Attempts to remove gold from the player. Returns true if successful.
    /// </summary>
    public bool RemoveGold(int amount)
    {
        if (GoldCount >= amount)
        {
            GoldCount -= amount;
            return true;
        }
        return false;
    }
}