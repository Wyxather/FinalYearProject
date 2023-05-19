#pragma once

class Object {
public:
    Object();

    virtual auto update() -> void;
    virtual auto draw() -> void;

    ImVec2 pos = {};
};

class PhysicObject : public Object {
public:
    virtual auto update() -> void override;

    ImVec2 speed = {};
    ImVec2 accel = {};
};