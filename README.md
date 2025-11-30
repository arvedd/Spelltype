# SpellType

<table>
  <tr>
    <td align="left" width="50%">
      <img width="100%" alt="gif1" src="https://media.giphy.com/media/Vxcamtr1OBzeSyHkR7/giphy.gif">
    </td>
    <!-- <td align="right" width="50%">
      <img width="100%" alt="gif2" src="https://github.com/user-attachments/assets/f96b78ce-3f23-4b2e-a17f-c7c1581d5cf5">
    </td> -->
  </tr>
</table>

**SpellType** is a 2D typing-based dungeon crawler game where you play as a young mage learning the forgotten Ancient Language.
To cast spells, you must type them correctly, as if reciting mantras from an ancient spellbook.

Explore dungeons, recover lost pages of the "Book Name", and face powerful enemies, including the ultimate test against your own mentor.

---

## Developer
- Ngakan Nyoman Arya Vedantha 
- Nicholas Andianto 
- Muhammad Rafi
<br>

## Features & Scripts 

<table>
  <tr>
    <th>Script</th>
    <th>Description</th>
  </tr>

  <tr><td>Attack.cs</td><td>Handles player or enemy attack behavior, including triggering damage logic.</td></tr>
  <tr><td>AudioManager.cs</td><td>Centralized manager for playing SFX and background music during gameplay.</td></tr>
  <tr><td>BattleSystem.cs</td><td>Controls turn-based battle flow, state transitions, and combat handling.</td></tr>
  <tr><td>Card.cs</td><td>Defines card data used in the spell/typing system.</td></tr>
  <tr><td>CardDisplay.cs</td><td>Displays card visuals such as name, art, and spell info in UI.</td></tr>
  <tr><td>CounterInput.cs</td><td>Reads and processes player typing input for casting spells.</td></tr>
  <tr><td>Damageable.cs</td><td>Interface/component that allows entities to receive damage.</td></tr>
  <tr><td>DeckManager.cs</td><td>Handles deck composition, card drawing, and card management.</td></tr>
  <tr><td>Enemy.cs</td><td>Controls enemy stats, damage handling, attacks, and death logic.</td></tr>
  <tr><td>EnemyBehavior.cs</td><td>Defines how enemies behave or react during battle.</td></tr>
  <tr><td>EnemyMovement.cs</td><td>Manages enemy movement logic outside or inside the dungeon.</td></tr>
  <tr><td>EnemySpawner.cs</td><td>Spawns enemies during gameplay/dungeon waves.</td></tr>
  <tr><td>GameStateManager.cs</td><td>Controls global game states such as pause, play, or transitions.</td></tr>
  <tr><td>GoldManager.cs</td><td>Stores and updates player gold for upgrades and shop systems.</td></tr>
  <tr><td>HandManager.cs</td><td>Displays and updates player hand during typing/spell casting.</td></tr>
  <tr><td>Healthbar.cs</td><td>Controls visual HP bar elements for player or enemies.</td></tr>
  <tr><td>Lifespan.cs</td><td>Destroys temporary objects after a set lifetime.</td></tr>
  <tr><td>MapDisplay.cs</td><td>Displays dungeon map previews or UI minimaps.</td></tr>
  <tr><td>MapManager.cs</td><td>Handles dungeon generation, map nodes, and navigation.</td></tr>
  <tr><td>PauseManager.cs</td><td>Controls pausing, resume logic, and pause UI display.</td></tr>
  <tr><td>PauseMenu.cs</td><td>UI script for pause menu buttons and interactions.</td></tr>
  <tr><td>Player.cs</td><td>Main script representing player stats, abilities, and state.</td></tr>
  <tr><td>PlayerController.cs</td><td>Handles player movement & controls.</td></tr>
  <tr><td>PlayerDeck.cs</td><td>Stores player’s deck and manages obtained spells.</td></tr>
  <tr><td>PlayerLevel.cs</td><td>Manages XP, leveling system, and upgrades.</td></tr>
  <tr><td>ReturnToMenu.cs</td><td>Handles returning to main menu from any scene.</td></tr>
  <tr><td>RewardManager.cs</td><td>Determines rewards after battles or dungeon progress.</td></tr>
  <tr><td>SceneChanger.cs</td><td>Handles transitions between scenes (fade, load, etc).</td></tr>
  <tr><td>ShopBook.cs</td><td>Displays list of spells in shop UI.</td></tr>
  <tr><td>ShopBuy.cs</td><td>Handles purchases in the spell shop.</td></tr>
  <tr><td>ShopItem.cs</td><td>Represents a buyable item in the shop system.</td></tr>
  <tr><td>ShopManager.cs</td><td>Controls overall shop interaction, UI, and currency checks.</td></tr>
  <tr><td>SpellBook.cs</td><td>Stores all spells available in the game.</td></tr>
  <tr><td>SpellTyper.cs</td><td>Core system for typing-based spell casting.</td></tr>
  <tr><td>UIManager.cs</td><td>Manages UI screens, health bars, and HUD elements.</td></tr>

</table>


---

## Files description

```
├── SpellType                         # Contain everything needed for SpellType to works.
   ├── Assets                         # Contains every assets that have been worked with unity to create the game like the scripts and the art.
      ├── Sprites                     # Contains all the game art like the sprites, background, even the character.
      ├── Fonts                       # Contains every font that have been used in the game.
      ├── Sounds                      # Contains every sound used for the game like music and sound effects.
      ├── Scripts                     # Contains all scripts needed to make the gane get goings like PlayerMovement scripts.
      ├── Scenes                      # Contains all scenes that exist in the game for it to interconnected with each other like MainMenu, Gameplay.
```

## Game controls

The following controls are bound in-game, for gameplay and testing.
| Key Binding       | Function                                       |
| ----------------- | ---------------------------------------------- |
| Keyboard Letters  | Type spells to attack enemies                  |
| Backspace         | Delete the last typed letter                   |
| Enter             | Submit the spell when the word is complete     |
| Esc               | Pause Menu                                     |
| Tab               | Inventory                                      |
| W A S D           | Pointer a Potion in Shop                       |
| R. Ctrl           | Go Back to Map Selection                       |


<br>

## Project Goal

This game was created to fulfill the **Capstone Project**.

## Play Game Here
Itch.io Link: <src>https://cynora.itch.io/spelltype
---
