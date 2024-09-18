using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace fletesProyect.driver
{
    public class driversHub : Hub
    {
        // Un ConcurrentDictionary para almacenar la posición de los drivers conectados
        private static ConcurrentDictionary<long, string> DriverPositions = new ConcurrentDictionary<long, string>();

        // Método para actualizar la posición del driver
        public async Task UpdatePosition(double latitude, double longitude)
        {
            string driverId = Context.User.Claims.FirstOrDefault(c => c.Type == "driverId")?.Value;

            if (!string.IsNullOrEmpty(driverId))
            {
                long parsedDriverId = long.Parse(driverId);
                DriverPositions[parsedDriverId] = $"{latitude},{longitude}";
            }
        }

        // Método para obtener la posición de un driver
        public string? GetDriverPosition(long driverId)
        {
            // if (DriverPositions.TryGetValue(driverId, out var position))
            // {
            //     return position;
            // }
            // return null;
            List<string> positions = new List<string>();
            positions.Add("16.906682, -89.940174");
            positions.Add("16.915402, -89.955067");
            positions.Add("16.930593, -89.931335");
            positions.Add("16.919754, -89.926014");
            positions.Add(null);
            return positions[(int)driverId];
        }

        // Cuando un driver se desconecta, puedes remover su posición
        public override Task OnDisconnectedAsync(Exception exception)
        {
            string driverId = Context.User.Claims.FirstOrDefault(c => c.Type == "driverId")?.Value;
            if (!string.IsNullOrEmpty(driverId))
            {
                long parsedDriverId = long.Parse(driverId);
                DriverPositions.TryRemove(parsedDriverId, out _);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}