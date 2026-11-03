# Alien Fleet

## Change history

1. **Fleet speed-up:** the fleet's sideways speed scales with the number of aliens destroyed, while drop distance stays fixed.
2. **Unwinnable detection:** the game ends when the ship is gone or an alien passes the firing line.
3. **Screen clamp:** the ship's edges stop exactly at the screen edges. Each side is measured separately because the ship's pivot is off-centre.
4. **Code cleanup:**
   - The counter updates only when the count changes, instead of every frame.
   - The fleet keeps a typed `List<Alien>` with cached colliders, instead of repeated `GetComponent` calls.
   - The fleet's movement is a single loop, and `Invoke` uses `nameof` instead of a hard-coded string.
   - Removed empty Unity methods, unused fields and imports, and the CS0414 warning.
5. **Balance pass:** tuned with a temporary test bot over about 50 games; the bot has since been removed.
   - Added the one-dart-at-a-time rule.
   - Darts got faster (0.3 → 0.5 per step).
   - The fleet drops further at each wall (0.5 → 0.6).
   - Result: the bot still won every game, but the aliens got within 2.7–6 units of the firing line, against 6–12 originally.
6. **Start and end screens**, with play-again.
7. **Lives:** 3 lives, a ship-icon HUD and post-hit invulnerability. The alien counter was also moved on-screen.
8. **Starfield background**, replacing the default sky-and-ground skybox.
9. **Shields:** four destructible shields.
