using Common.Events.Drone;
using Common.Events.Session;

namespace Common.Services.EventHandlers
{
    public delegate void SessionEventHandler(object sender, SessionEventArgs e);
    public delegate void DroneEventHandler(object sender, DroneEventArgs e);
}
