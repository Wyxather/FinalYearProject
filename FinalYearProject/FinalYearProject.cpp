#include "Source/Game.h"

auto APIENTRY wWinMain(
    _In_        const HINSTANCE hInstance,
    _In_opt_    const HINSTANCE hPrevInstance,
    _In_        const LPWSTR    lpCmdLine,
    _In_        const int       nCmdShow
) -> int {
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    game = std::make_unique<decltype(game)::element_type>(hInstance, nCmdShow);

    return game->update();
}