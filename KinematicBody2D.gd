extends KinematicBody2D

export var id = 0
var wheel_base = 70
var steering_angle = 50
var engine_power = 800
var friction = -0.9 #baseado na vel - slow speed
var drag = -0.001 #baseado na square of your vel - high speed
var braking = -450 #metade da engine +-
var max_speed_reverse = 250
var slip_speed = 400
var traction_fast = 0.1 #slindind
var traction_slow = 0.7 #sliding

var acceleration = Vector2.ZERO
var velocity = Vector2.ZERO
var steer_direction

var pontos1 = 0
var aux_pontos1 = 0


func _physics_process(delta):
	acceleration = Vector2.ZERO
	get_input()
	apply_friction()
	calculate_steering(delta)
	velocity += acceleration * delta
	if (velocity.x > 300 or velocity.y < -300):
		$Sprite.play("RAPIDO")
	elif ((velocity.x <= 300) and (velocity.x >= 51)) or ((velocity.y <= -51) and (velocity.y >= -300)):
		$Sprite.play("MEDIO")
	elif ((velocity.x < 50) and (velocity.x > -50)) or ((velocity.y < 50) and (velocity.y > -50)):
		$Sprite.play("DEVAGAR")
	velocity = move_and_slide(velocity)
	
func apply_friction():
	if velocity.length() < 5:
		velocity = Vector2.ZERO
	var friction_force = velocity * friction #friction é negativo, vai numa direção opposta
	var drag_force = velocity * velocity.length() * drag
	acceleration += drag_force + friction_force

func get_input():
	var turn = 0
	if Input.is_action_pressed('right_%s' % id):
		turn += 1
	elif Input.is_action_pressed('left_%s' % id):
		turn -= 1
	steer_direction = turn * deg2rad(steering_angle)
	#se tirar aqui ele vai direto
	if Input.is_action_pressed('up_%s' % id):
		acceleration = transform.x * engine_power
	if Input.is_action_pressed('down_%s' % id):
		acceleration = transform.x * braking
		
func calculate_steering(delta):
	var rear_wheel = position - transform.x * wheel_base/2.0
	var front_wheel = position + transform.x * wheel_base/2.0
	rear_wheel += velocity * delta
	front_wheel += velocity.rotated(steer_direction) * delta
	var new_heading = (front_wheel - rear_wheel).normalized()
	var d = new_heading.dot(velocity.normalized())
	var traction = traction_slow
	if velocity.length() > slip_speed:
		traction = traction_fast
	if d > 0:
		velocity = velocity.linear_interpolate(new_heading * velocity.length(), traction) #based on traction
	if d < 0:
		velocity = -new_heading * min(velocity.length(), max_speed_reverse) 
	rotation = new_heading.angle()
	
	pass

func _on_LinhaChegada_area_exited(area):
	aux_pontos1 = 0
	pass

func _on_Area2D_area_entered(area):
	if(aux_pontos1 == 0):
		pontos1 += 1
	print(pontos1)
	aux_pontos1 += 1
	pass # Replace with function body.

