extends Node

onready var DisplayValue = get_node("numero/Label")
var contador = 0
var ch = 0
#precisa prepara a variavel timer antes de chamar pra _ready():
onready var timer = get_node("Timer")

func _process(delta):
	#pausa o jogo
	if ch == 0:
		get_tree().paused = true
	if contador == 4:
		get_tree().paused = false
		ch = 1
	#despausa o jogo
	pass
func _ready():
	#a cada 2s chama a funcao timeout
	
	timer.set_wait_time(2)
	timer.start()

func _on_Timer_timeout():
	contador += 1
	if contador == 5:#no proximo contagem,libera o label
		queue_free()
	DisplayValue.text = str(contador)
	print(DisplayValue.text)
	if contador == 4:
		DisplayValue.text = "GO!"		 
	pass # Replace with function body.


