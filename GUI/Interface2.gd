extends Control
#onready var world = $Viewports/ViewportContainer1/Viewport1/World
onready var world = get_tree().get_root().get_node("Main/Viewports/ViewportContainer1/Viewport1/World")
#3 tentativa,onready var
#preload

func _process(delta):	
	var player = world.get_node("Player2")
	var LabelNode = get_parent().get_node("interface2/Counter/Label")
	var ponto = player.get("pontos2")
	LabelNode.text = str(ponto)
	#print(LabelNode.text)
	pass
	
func _ready():
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass


