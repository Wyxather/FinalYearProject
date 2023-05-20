#include "Object.h"

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

auto Player::draw() -> void {
    Entity::draw();

    ImGui::Begin("Action");
    if (ImGui::Button("Shoot")) {
        isShooting = false;
        pendingShoot = true;
    }
    if (ImGui::IsItemClicked()) {
        isShooting = true;
        power = 0.f;
    }
    if (isShooting) {
        power += 25.f * ImGui::GetIO().DeltaTime;
        if (power > 100.f) {
            power = 100.f;
        }
    }
    ImGui::Text(isShooting ? "isShooting: true" : "isShooting: false");
    ImGui::Text("Power: %.0f", power);
    ImGui::End();

    const auto& displaySize = ImGui::GetIO().DisplaySize;
    ImGui::GetBackgroundDrawList()->AddRect(
        ImVec2{},
        ImVec2{ displaySize.x, 20.f },
        IM_COL32(255, 0, 0, 255),
        1.0f,
        ImDrawFlags_RoundCornersAll
    );
    ImGui::GetBackgroundDrawList()->AddRectFilled(
        ImVec2{},
        ImVec2{ displaySize.x * (power * .01f), 20.f },
        IM_COL32(255, 0, 0, 255),
        1.0f,
        ImDrawFlags_RoundCornersAll
    );
}

auto Player::update() -> void {
    if (ImGui::IsKeyPressed(ImGui::GetKeyIndex(ImGuiKey_LeftArrow))) {
        pos.x -= 1.f;
    } else if (ImGui::IsKeyPressed(ImGui::GetKeyIndex(ImGuiKey_RightArrow))) {
        pos.x += 1.f;
    }
}