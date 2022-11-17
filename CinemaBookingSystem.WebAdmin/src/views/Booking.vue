<template>
  <section class="layout">
    <div class="sidebar"><Navbar/></div>
    <div class="body">
      <div class="row">
        <Menubar1/>
      </div>
      <div class="row p-2">
        <card>
          <template #title> Booking Information </template>
          <template #content>
            <div class="card">
              <h5>Personal information</h5>
              <div class="field grid">
                <label for="firstName" class="col-12 mb-2 md:col-2 md:mb-0"
                  >First name</label
                >
                <div class="col-12 md:col-10">
                  <input
                    id="firstName"
                    type="text"
                    class="
                      text-base text-color
                      surface-overlay
                      p-2
                      border-1 border-solid
                      surface-border
                      border-round
                      appearance-none
                      outline-none
                      focus:border-primary
                      w-full
                    "
                    v-model="booking.personalName.firstName"
                    disabled
                  />
                </div>
              </div>
              <div class="field grid">
                <label for="lastName" class="col-12 mb-2 md:col-2 md:mb-0"
                  >Last name</label
                >
                <div class="col-12 md:col-10">
                  <input
                    id="lastName"
                    type="text"
                    class="
                      text-base text-color
                      surface-overlay
                      p-2
                      border-1 border-solid
                      surface-border
                      border-round
                      appearance-none
                      outline-none
                      focus:border-primary
                      w-full
                    "
                    v-model="booking.personalName.lastName"
                    disabled
                  />
                </div>
              </div>
              <div class="field grid">
                <label for="phoneNumber" class="col-12 mb-2 md:col-2 md:mb-0"
                  >Phone number</label
                >
                <div class="col-12 md:col-10">
                  <input
                    id="phoneNumber"
                    type="text"
                    class="
                      text-base text-color
                      surface-overlay
                      p-2
                      border-1 border-solid
                      surface-border
                      border-round
                      appearance-none
                      outline-none
                      focus:border-primary
                      w-full
                    "
                    v-model="booking.personalName.phoneNumber"
                    disabled
                  />
                </div>
              </div>
              <h5>Seance information</h5>
              <div class="field grid">
                <label for="date" class="col-12 mb-2 md:col-2 md:mb-0"
                  >Date</label
                >
                <div class="col-12 md:col-10">
                  <input
                    id="date"
                    type="text"
                    class="
                      text-base text-color
                      surface-overlay
                      p-2
                      border-1 border-solid
                      surface-border
                      border-round
                      appearance-none
                      outline-none
                      focus:border-primary
                      w-full
                    "
                    v-model="booking.seance.date"
                    disabled
                  />
                </div>
              </div>
              <div class="field grid">
                <label for="movieTitle" class="col-12 mb-2 md:col-2 md:mb-0"
                  >Movie title</label
                >
                <div class="col-12 md:col-10">
                  <input
                    id="movieTitle"
                    type="text"
                    class="
                      text-base text-color
                      surface-overlay
                      p-2
                      border-1 border-solid
                      surface-border
                      border-round
                      appearance-none
                      outline-none
                      focus:border-primary
                      w-full
                    "
                    v-model="booking.seance.date"
                    disabled
                  />
                </div>
              </div>
              <h5>Tickets</h5>
              <li v-for="item in booking.seanceSeats" :key="item.id">
                <span
                  >Seat number: {{ item.cinemaSeat.seatNumber }} Row:
                  {{ item.cinemaSeat.row }} Type:
                  {{ item.cinemaSeat.seatType }}</span
                >
              </li>
            </div>
            <div class="text-right row mt-5">
              <Button
                label="Back"
                icon="pi pi-arrow-left"
                iconPos="left"
                class="p-button-info mr-2"
                @click="goBack()"
              />
              <Button
                label="Delete"
                icon="pi pi-trash"
                @click="openConfirmation"
                class="p-button-danger"
              />
              <Dialog
                header="Delete"
                v-model:visible="displayConfirmation"
                :style="{ width: '350px' }"
                :modal="true"
              >
                <div class="confirmation-content">
                  <i
                    class="pi pi-exclamation-triangle mr-3"
                    style="font-size: 2rem"
                  />
                  <span>Are you sure you want to delete ?</span>
                </div>
                <template #footer>
                  <Button
                    label="No"
                    icon="pi pi-times"
                    @click="closeConfirmation"
                    class="p-button-text"
                  />
                  <Button
                    label="Yes"
                    icon="pi pi-check"
                    @click="deleteBooking(booking.id)"
                    class="p-button-text"
                    autofocus
                  />
                </template>
              </Dialog>
            </div>
          </template>
        </card>
      </div>
    </div>
  </section>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import Menubar1 from "@/components/Menubar.vue";
import Button from "primevue/button";
import Dialog from "primevue/dialog";
import BookingService from "@/services/booking-service";
import { useRoute } from "vue-router";
import Card from "primevue/card";
export default {
  name: "BookingView",
  components: {
    Button,
    Dialog,
    Card,
    Navbar,
    Menubar1,
  },
  data() {
    return {
      displayConfirmation: false,
      displayAddHall: false,
      active: 1,
      booking: {
        personalName: {
            firstName: '',
            lastName: '',
            phoneNumber: ''
        },
        seance: {
            date: null
        }
      },
      showError: false,
      errorMessage: "",
    };
  },
  created() {
    const route = useRoute();
    const id = route.params.id;
    BookingService.get(id)
      .then((response) => {
        this.booking = response.data;
      })
      .catch((error) => {
        if (error.response.status === 404) {
          this.$router.replace({
            name: "NotFound",
            params: { err: error.response.data.Message },
          });
        }
      });
  },
  methods: {
    openConfirmation() {
      this.displayConfirmation = true;
    },
    closeConfirmation() {
      this.displayConfirmation = false;
    },
    goBack() {
      this.$router.replace({ name: "Bookings" });
    },
    deleteBooking(id) {
      BookingService.delete(id)
        .then((response) => {
          console.log(response.data);
          this.$router.replace({ name: "Bookings" });
        })
        .catch((error) => {
          console.log(error.response.data);
        });
      this.displayConfirmation = false;
    },
  },
};
</script>

<style scoped lang="scss">
a {
  text-decoration: none;
  color: black;
}
.field * {
  display: block;
}
[v-cloak] {
  display: none;
}
.layout {
  width: 100%;
  display: grid;
  grid:
    "sidebar body" 1fr
    / 15% 85%;
  gap: 8px;
}
.sidebar {
  grid-area: sidebar;
  min-height: 100vh;
  border-right: 1px solid lightgray;
}
.body {
  grid-area: body;
}
</style>