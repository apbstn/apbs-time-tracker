<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue';
import { useLayout } from '@/layout/composables/layout';

const { layoutState } = useLayout();

// Reactive notifications array
const notifications = ref([]);  
const showNotifications = ref(false);

function toggleNotifications() {
  showNotifications.value = !showNotifications.value;
}

// Function to add a notification dynamically
function addNotification(message) {
  if (message) {
    notifications.value.unshift({ text: message, id: Date.now() }); // Store notifications as objects
    if (notifications.value.length > 5) notifications.value.pop(); // Keep max 5
  }
}

// Close notifications when clicking outside
function closeNotifications(event) {
  if (!event.target.closest('.notification-container')) {
    showNotifications.value = false;
  }
}

onMounted(() => {
  document.addEventListener("click", closeNotifications);
});

onBeforeUnmount(() => {
  document.removeEventListener("click", closeNotifications);
});

// ✅ Expose the addNotification function for global testing
window.addNotification = addNotification;
</script>

<template>
  <div class="layout-topbar">
    <div class="layout-topbar-end">
      <ul class="layout-topbar-items">
        <li class="notification-container">
          <button @click.stop="toggleNotifications" class="notification-button">
            <span class="p-overlay-badge">
              <i class="pi pi-bell"></i>
              <span v-if="notifications.length" class="p-badge p-badge-danger">{{ notifications.length }}</span>
            </span>
          </button>

          <!-- Notification Dropdown -->
          <div v-if="showNotifications" class="notification-dropdown">
            <h4 class="text-lg font-semibold mb-2">Notifications</h4>
            <ul v-if="notifications.length">
              <li v-for="(notif, index) in notifications" :key="notif.id" class="p-2 border-b last:border-none">
                {{ notif.text }}
              </li>
            </ul>
            <p v-else class="text-gray-500">No notifications</p>
          </div>
        </li>
      </ul>
    </div>
  </div>
</template>

<style scoped>
/* Ensure topbar and bell align correctly */
.layout-topbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
  padding: 0 20px;
  background: #333;
  color: rgb(0, 0, 0);
}

.layout-topbar-end {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  width: 100%;
}

/* Notification Bell */
.notification-container {
  position: absolute;
  right: 20px;
  top: 10px;
}

.notification-button {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.5rem;
  color: white;
  position: relative;
}

/* Notification Dropdown */
.notification-dropdown {
  position: absolute;
  top: 40px;
  right: 0;
  background: white;
  border: 1px solid #ddd;
  border-radius: 5px;
  padding: 10px;
  box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.2);
  width: 250px;
  z-index: 9999;
}

/* Notification Items */
.notification-dropdown ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.notification-dropdown li {
  padding: 8px 10px;
  border-bottom: 1px solid #eee;
  font-size: 14px;
}

.notification-dropdown li:last-child {
  border-bottom: none;
}

/* Badge Style */
.p-badge {
  position: absolute;
  top: -5px;
  right: -5px;
  font-size: 12px;
  padding: 3px 6px;
  border-radius: 50%;
}
</style>
