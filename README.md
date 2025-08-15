基于动画状态的角色控制器；

核心脚本：
  BaseLayerAnimBehaviour —— 用于监听整体Animator动画信息；
  AnimBehaviourBase —— 用于通过Animator BaseLayer透传；
  ComboControllerBase —— 用于连招控制，预输入，招式CD，招式间隔CD；
  PlayerMoveController —— 用于移动控制，与招式控制隔离；

核心SO数据：
  SOAnimation —— 保存对应角色动画信息；
  SOAttribute —— 保存对应角色属性信息；
  SOComboConfig —— 最小招式数据；
  SOComboList —— 招式集合数据；

配置流程：
  1：创建角色对应的Animator数据，并在BaseLayer层挂载脚本 “BaseLayerAnimBehaviour” 用于透传动画信息
  2：在控制角色身上挂载连招控制脚本（PlayerComboController）、移动控制脚本（PlayerMoveController）
