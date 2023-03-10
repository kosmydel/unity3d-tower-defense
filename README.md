# Tower Defense in Unity3D

## Screenshots
### Main menu
![Main menu](/images/0.PNG)

### Game
You start the game with given amount of lives and money. Then your aim is to survive all waves by using canoons.

![Game](/images/1.PNG)

### Upgrading panel
There is built-in upgrading module. You can add upgrades to each canoon by specyfing upgrade cost and upgraded canoon prefab.

![Upgrade](/images/2.PNG)

### Many types of baloons
You can easily add new types of baloons by creating new prefab and registering it in the GameManager. There are default baloon parameters which you can adjust (like health, money gained for killing, speed) in the `MonsterScript` abstract.

![Game](/images/3.PNG)

### Extra bonuses
There are 2 default bonuses: wall and bomb which can help you win the game. Be careful, there is colldown between usages!

![Bonuses](/images/4.PNG)

## Other
**Configurable shop** - configuration of shop is easy. To add item to the shop just use prefab with configurable parameters. Remember that `Item` has to use abstract class `ShopItem`.

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
