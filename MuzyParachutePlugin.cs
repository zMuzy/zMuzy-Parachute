using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Events;

namespace MuzyParachute;

[MinimumApiVersion(80)]
public class MuzyParachutePlugin : BasePlugin
{
    private const float ParachuteGravityScale = 0.1f;
    private const float NormalGravityScale = 1.0f;
    private const float FallSpeed = -30.0f;
    private const float MinimumFallingSpeedToStart = -10.0f;

    private readonly HashSet<uint> _parachutingSlots = [];

    public override string ModuleName => "zMuzy Parachute";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Made by zMuzy (with AI assistance)";
    public override string ModuleDescription => "Hold E (+use) while falling to glide down slowly.";

    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnTick>(OnTick);
        RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
        RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath);
        RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect);
        RegisterEventHandler<EventRoundStart>(OnRoundStart);

        if (!hotReload)
            return;

        foreach (var player in Utilities.GetPlayers())
            ResetParachute(player);
    }

    public override void Unload(bool hotReload)
    {
        foreach (var player in Utilities.GetPlayers())
            ResetParachute(player);

        _parachutingSlots.Clear();
    }

    private HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        ResetParachute(@event.Userid);
        return HookResult.Continue;
    }

    private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        ResetParachute(@event.Userid);
        return HookResult.Continue;
    }

    private HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player == null)
            return HookResult.Continue;

        _parachutingSlots.Remove(player.Index);
        return HookResult.Continue;
    }

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        foreach (var player in Utilities.GetPlayers())
            ResetParachute(player);

        return HookResult.Continue;
    }

    private void OnTick()
    {
        foreach (var player in Utilities.GetPlayers())
        {
            if (!TryGetUsablePawn(player, out var pawn))
            {
                ResetParachute(player);
                continue;
            }

            if (!ShouldParachute(player!, pawn!))
            {
                ResetParachute(player);
                continue;
            }

            StartParachute(player!, pawn!);
        }
    }

    private static bool TryGetUsablePawn(CCSPlayerController? player, out CCSPlayerPawn? pawn)
    {
        pawn = null;

        if (player == null || !player.IsValid || player.IsBot || !player.PawnIsAlive)
            return false;

        pawn = player.PlayerPawn.Value;
        return pawn != null && pawn.IsValid;
    }

    private static bool ShouldParachute(CCSPlayerController player, CCSPlayerPawn pawn)
    {
        if ((player.Buttons & PlayerButtons.Use) == 0)
            return false;

        if (pawn.GroundEntity.IsValid)
            return false;

        return pawn.AbsVelocity.Z <= MinimumFallingSpeedToStart;
    }

    private void StartParachute(CCSPlayerController player, CCSPlayerPawn pawn)
    {
        _parachutingSlots.Add(player.Index);

        if (Math.Abs(pawn.GravityScale - ParachuteGravityScale) > 0.001f)
            pawn.GravityScale = ParachuteGravityScale;

        var velocity = pawn.AbsVelocity;
        if (velocity.Z < FallSpeed)
            velocity.Z = FallSpeed;
    }

    private void ResetParachute(CCSPlayerController? player)
    {
        if (player == null)
            return;

        _parachutingSlots.Remove(player.Index);

        if (!TryGetUsablePawn(player, out var pawn))
            return;

        if (Math.Abs(pawn!.GravityScale - NormalGravityScale) > 0.001f)
            pawn.GravityScale = NormalGravityScale;
    }
}
