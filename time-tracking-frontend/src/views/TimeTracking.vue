<template>
    <div class="time-tracking">
      <h2>Time Tracking</h2>
      <button @click="startTracking">Start</button>
      <button @click="pauseTracking">Pause</button>
      <button @click="stopTracking">Stop</button>
    </div>
  </template>
  
  <script>
  import api from "@/api";
  
  export default {
    methods: {
      async startTracking() {
        const token = localStorage.getItem("token");
        await api.post("/timelog/start", {}, { headers: { Authorization: `Bearer ${token}` } });
      },
      async pauseTracking() {
        const token = localStorage.getItem("token");
        await api.post("/timelog/pause", {}, { headers: { Authorization: `Bearer ${token}` } });
      },
      async stopTracking() {
        const token = localStorage.getItem("token");
        await api.post("/timelog/stop", {}, { headers: { Authorization: `Bearer ${token}` } });
      },
    },
  };
  </script>
  