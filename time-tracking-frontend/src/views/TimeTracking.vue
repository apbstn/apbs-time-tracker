<template>
  <div class="time-tracking">
    <h2>Time Tracking</h2>
    <div class="buttons">
      <button @click="startTracking">Start</button>
      <button @click="pauseTracking">Pause</button>
      <button @click="stopTracking">Stop</button>
    </div>
  </div>
</template>

<script>
import api from "@/api";

export default {
  data() {
    return {
      currentStatus: "",
    };
  },
  methods: {
    async startTracking() {
      console.log("Start button clicked");

      // ✅ Always fetch the latest token
      const token = localStorage.getItem("accessToken");
      if (!token) {
        alert("You're not authenticated. Please log in again.");
        return;
      }

      console.log("Stored Token:", token);

      const idInput = prompt("Enter the ID (number):");
      const id = idInput ? parseInt(idInput, 10) : null;
      if (id === null || isNaN(id)) {
        alert("Invalid ID. Please enter a valid number.");
        return;
      }

      try {
        await api.post(`api/timelog/start/${id}`, {}, {
          headers: { Authorization: `Bearer ${token}` }
        });
        this.currentStatus = "started";
      } catch (error) {
        console.error("Error starting tracking:", error.response?.data || error.message);
      }
    },

    async pauseTracking() {
      console.log("Pause button clicked");

      if (this.currentStatus !== "started") {
        alert("You can only pause after starting.");
        return;
      }

      const token = localStorage.getItem("accessToken");
      if (!token) {
        alert("You're not authenticated. Please log in again.");
        return;
      }

      console.log("Stored Token:", token);

      const idInput = prompt("Enter the ID (number):");
      const id = idInput ? parseInt(idInput, 10) : null;
      if (id === null || isNaN(id)) {
        alert("Invalid ID. Please enter a valid number.");
        return;
      }

      try {
        await api.post(`api/timelog/pause/${id}`, {}, {
          headers: { Authorization: `Bearer ${token}` }
        });
        this.currentStatus = "paused";
      } catch (error) {
        console.error("Error pausing tracking:", error.response?.data || error.message);
      }
    },

    async stopTracking() {
      console.log("Stop button clicked");

      if (this.currentStatus !== "started" && this.currentStatus !== "paused") {
        alert("You can only stop after starting or pausing.");
        return;
      }

      const token = localStorage.getItem("accessToken");
      if (!token) {
        alert("You're not authenticated. Please log in again.");
        return;
      }

      console.log("Stored Token:", token);

      const idInput = prompt("Enter the ID (number):");
      const id = idInput ? parseInt(idInput, 10) : null;
      if (id === null || isNaN(id)) {
        alert("Invalid ID. Please enter a valid number.");
        return;
      }

      try {
        await api.post(`api/timelog/stop/${id}`, {}, {
          headers: { Authorization: `Bearer ${token}` }
        });
        this.currentStatus = "stopped";
      } catch (error) {
        console.error("Error stopping tracking:", error.response?.data || error.message);
      }
    }
  }
};
</script>

<style scoped>
.time-tracking {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  text-align: center;
}

.buttons {
  display: flex;
  gap: 1rem;
}

button {
  padding: 0.5rem 1rem;
  font-size: 1rem;
  cursor: pointer;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
}

button:hover {
  background-color: #0056b3;
}
</style>
