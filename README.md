# Tower Defense in Unity3D

## Screenshots
### Main menu
![Main menu](/images/0.PNG)

### Game
You start the game with a given amount of lives and money. Then you aim to survive all waves by using cannons.

![Game](/images/1.PNG)

### Upgrading panel
There is a built-in upgrading module. You can add upgrades to each cannon by specifying upgrade cost and upgraded cannon prefab.

![Upgrade](/images/2.PNG)

### Many types of balloons
You can easily add new types of balloons by creating a new prefab and registering it in the GameManager. There are default balloon parameters that you can adjust (like health, the money gained for killing, and speed) in the `MonsterScript` abstract.

![Game](/images/3.PNG)

### Extra bonuses
There are 2 default bonuses: wall and bomb which can help you win the game. Be careful, there is a cooldown between usages!

![Bonuses](/images/4.PNG)

## Other
**Configurable shop** - configuration of the shop is easy. To add an item to the shop just use a prefab with configurable parameters. Remember that `Item` has to use the abstract class `ShopItem`.

![Bonuses](/images/5.PNG)


**Configurable rounds** - you can easily configure rounds using JSON.
```json
{
  "name": "Poziom łatwy",
  "startingMoney": 100,
  "startingHealth": 20,
  "waves": [
    {
      "id": 1,
      "monsters": [
        {
          "monsterID": 0,
          "amount": 5,
          "delay": 10
        }
      ]
    },
    {...}
  ]
}
```
