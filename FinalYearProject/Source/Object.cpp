#include "Object.h"

Object::Object() {
    pos = ImGui::GetIO().MouseClickedPos[0];
}

auto Object::update()-> void {}

auto Object::draw() -> void {
    ImGui::GetBackgroundDrawList()->AddCircle(pos, 20.0f, IM_COL32(255, 0, 0, 255));
}

auto PhysicObject::update() -> void {
    Object::update();

    accel.y += 1.0f * ImGui::GetIO().DeltaTime;
    speed += accel;
    pos += speed;
}