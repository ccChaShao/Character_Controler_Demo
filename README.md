基于动画状态的角色控制器；

核心脚本：
  BaseLayerAnimBehaviour —— 用于监听整体Animator动画信息；
  AnimBehaviourBase —— 用于通过Animator BaseLayer透传；
  ComboControllerBase —— 用于连招控制，预输入，招式CD，招式间隔CD，动画事件监听；
  PlayerMoveController —— 用于移动控制，与招式控制隔离；

核心SO数据：
  SOAnimation —— 保存对应角色动画信息；
  SOAttribute —— 保存对应角色属性信息；
  SOComboConfig —— 最小招式数据；
  SOComboList —— 招式集合数据；

最小配置流程：
  1：创建角色对应的Animator，并在BaseLayer层挂载脚本 “BaseLayerAnimBehaviour” 用于透传动画信息；
  2：创建角色对应的Animation Clip，并在对应的关键帧上添加譬如预输入、特效、音效、hitbox等动画事件；
  3：创建对应的连招数据，并由SOComboList统一管理；
  4：在控制角色身上挂载连招控制脚本（PlayerComboController）、移动控制脚本（PlayerMoveController）；

后续开发方向：
  1：根据预留的动画事件回调，处理特效、音效、hitbox、镜头抖动；
  2：将Animation框架改为Playable，并做到按需加载动画；
