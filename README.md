# Cat Playground

一个使用Unity开发的2D塔防/生存射击游戏。玩家控制猫咪角色，使用猫爪射击和爆炸技能对抗不断涌来的敌人，通过击杀敌人获得经验升级，挑战多个关卡。

## 游戏特色

- **双技能战斗系统**：射击（猫爪）+ 范围爆炸，可升级提升威力
- **经验升级系统**：击杀敌人掉落宝石，收集经验升级解锁更强技能
- **多关卡挑战**：每关独立配置敌人类型、数量和Boss
- **Boss战**：拥有多种攻击模式的Boss敌人（4/8方向射击、毒气攻击）
- **多平台输入**：支持键鼠、手柄（PS5）、触屏和XR设备

## 技术信息

- **Unity版本**: 6000.0.41f1
- **渲染管线**: Universal Render Pipeline (URP)
- **输入系统**: Unity Input System
- **摄像机**: Cinemachine

## 游戏玩法

### 操作方式

**键鼠控制**：
- WASD - 移动
- 鼠标移动 - 瞄准方向
- 鼠标左键 - 射击（猫爪）
- 鼠标右键 - 爆炸技能
- Shift - 冲刺

**手柄控制（PS5）**：
- 左摇杆 - 移动
- 右摇杆 - 瞄准方向
- ✖ (Cross) - 射击
- ⭕ (Circle) - 爆炸
- L2 - 冲刺

### 游戏机制

- 击杀敌人掉落宝石，自动吸附到玩家
- 收集宝石获得经验，经验满后升级
- 升级时可选择：升级左技能、升级右技能、增加HP
- 每关有击杀目标，达成后进入下一关
- 技能等级在关卡间保留，重新开局时重置

## 开发环境要求

- Unity 6000.0.41f1 或更高版本
- 支持的操作系统：Windows / macOS / Linux

## 如何开始

1. 克隆此仓库：
   ```bash
   git clone https://github.com/SakuseWen/Cat-Playground.git
   ```

2. 使用Unity Hub打开项目

3. 确保安装了正确的Unity版本（6000.0.41f1）

4. 打开项目后，Unity会自动导入所需的包和资源

5. 打开 `Assets/Scenes/StartUi.unity` 场景开始游戏

## 项目结构

```
Cat-Playground/
├── Assets/
│   ├── Art/
│   │   ├── scripts/        # 游戏逻辑脚本（45个脚本）
│   │   ├── Enemy/          # 敌人动画和预制体
│   │   ├── Bullet/         # 子弹和技能特效
│   │   ├── Player/         # 玩家角色资源
│   │   ├── Environment/    # 场景环境资源
│   │   └── UI/             # UI界面资源
│   ├── Prefabs/            # 游戏预制体
│   ├── Scenes/             # 游戏场景
│   └── Settings/           # 项目设置
├── Packages/               # Unity包管理
├── ProjectSettings/        # 项目配置文件
└── README.md
```

## 核心系统

### 游戏管理
- **GameManager** - 关卡流程、UI面板、摄像机管理
- **Spawner** - 敌人生成器，支持普通敌人和Boss定时生成
- **AudioManager** - 音频管理（BGM和音效）

### 玩家系统
- **Player** - 玩家控制、移动、技能释放
- **PlayerBeHit** - HP管理、受击反馈
- **LevelUp** - 经验值系统、升级面板

### 敌人系统
- **Enemy** - 普通敌人AI（追踪玩家）
- **Boss** - Boss敌人（多种攻击模式）
- **EnemyBeHit** - 伤害处理、掉落宝石

### 技能系统
- **SkillDefine** - 技能定义、多等级配置、跨场景保存
- **Bullet** - 射击子弹
- **ExplodeSkill** - 爆炸技能
- **EnemyBullet** - Boss子弹
- **PoisonGas** - Boss毒气攻击

## 贡献

欢迎提交Issue和Pull Request！

## 许可证

请查看LICENSE文件了解详情。

---

Made with ❤️ using Unity
