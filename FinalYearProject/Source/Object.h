#pragma once

class Collision {
public:
    Collision() {}

    Collision(Collision&&) = delete;
    Collision& operator=(Collision&&) = delete;

    Collision(const Collision&) = delete;
    Collision& operator=(const Collision&) = delete;

    virtual ~Collision() {}

    float radius = 0.f;
};

class Object {
public:
    Object(const ImVec2& pos) : pos{ pos } {}

    virtual ~Object() {}
    virtual auto update() -> void;
    virtual auto draw() -> void;

    ImVec2 pos = {};

    static inline std::vector<std::unique_ptr<Object>> objects;
    static inline std::vector<std::unique_ptr<Object>> spawnedObjects;
};

class PhysicObject : public Object {
public:
    PhysicObject(const ImVec2& pos) : Object{ pos } {}

    virtual ~PhysicObject() override {}
    virtual auto update() -> void override;

    ImVec2 speed = {};
    ImVec2 accel = {};
};

class Projectile : public PhysicObject {
public:
    virtual ~Projectile() override {}
};

class Entity : public Object {
public:
    Entity(const ImVec2& pos) : Object{ pos } {}

    virtual ~Entity() override {}
    virtual auto draw() -> void override;
};

class Player : public Entity {
public:
    Player(const ImVec2& pos) : Entity{ pos } {}

    virtual ~Player() override {}
    virtual auto update() -> void override;
    virtual auto draw() -> void override;

    bool isShooting = false;
    bool pendingShoot = false;
    float power = 0.f;
};