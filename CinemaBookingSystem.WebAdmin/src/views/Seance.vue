<template>
  <section class="layout">
    <div class="sidebar"><Navbar /></div>
    <div class="body">
      <div class="row">
        <Menubar1 />
      </div>
      <div class="row p-2">
        <card class="mt-3">
          <template #title> Edit seance </template>
          <template #content>
            <div class="ml-2">
              <Message severity="error" v-if="showError" :closable="false">{{
                errorMessage
              }}</Message>
              <div class="card">
                <h5>General information</h5>
                <label for="seanceDate" class="col-12 mb-2 md:col-2 md:mb-0"
                  >Seance date</label
                >
                <div class="col-12 md:col-10">
                  <Calendar
                    id="seanceDate"
                    v-model="seance.date"
                    :showTime="true"
                    :showSeconds="false"
                  />
                </div>
                <label for="seanceType" class="col-12 mb-2 md:col-2 md:mb-0"
                  >Seance type</label
                >
                <div class="col-12 md:col-10">
                  <Dropdown
                    v-model="seance.seanceType"
                    :options="seanceTypes"
                    optionValue="code"
                    optionLabel="name"
                    placeholder="Select a seance type"
                    required
                  />
                </div>
                <h5>Hall information</h5>
                <label for="cinema" class="col-12 mb-2 md:col-2 md:mb-0"
                  >Cinema</label
                >
                <div class="col-12 md:col-10">
                  <AutoComplete
                    v-model="selectedCinema"
                    :suggestions="filteredCinemas"
                    @complete="searchCinema($event)"
                    field="name"
                    :dropdown="true"
                  />
                </div>
                <label
                  for="hall"
                  class="col-12 mb-2 md:col-2 md:mb-0"
                  v-if="selectedCinema"
                  >Cinema Hall</label
                >
                <div class="col-12 md:col-10" v-if="selectedCinema">
                  <AutoComplete
                    v-model="selectedCinemaHall"
                    :suggestions="filteredCinemaHalls"
                    @complete="searchCinemaHall($event)"
                    field="name"
                    :dropdown="true"
                  />
                </div>

                <h5>Movie information</h5>
                <label for="movie" class="col-12 mb-2 md:col-2 md:mb-0"
                  >Movie</label
                >
                <div class="col-12 md:col-10">
                  <AutoComplete
                    v-model="selectedMovie"
                    :suggestions="filteredMovies"
                    @complete="searchMovie($event)"
                    field="title"
                    :dropdown="true"
                  />
                </div>
              </div>
              <div class="text-right row mt-5">
                <Button
                  label="Submit"
                  icon="pi pi-check"
                  iconPos="left"
                  class="p-button-success mr-2"
                  @click="submit()"
                />
                <Button
                  label="Cancel"
                  icon="pi pi-times"
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
                      @click="deleteSeance(seance.id)"
                      class="p-button-text"
                      autofocus
                    />
                  </template>
                </Dialog>
              </div>
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
import Message from "primevue/message";
import Button from "primevue/button";
import Dialog from "primevue/dialog";
import CinemaHallService from "@/services/cinema-hall-service";
import CinemaService from "@/services/cinema-service";
import MovieService from "@/services/movie-service";
import SeanceService from "@/services/seance-service";
import { useRoute } from "vue-router";
import Card from "primevue/card";
import Calendar from "primevue/calendar";
import Dropdown from "primevue/dropdown";
import AutoComplete from "primevue/autocomplete";
export default {
  name: "SeanceView",
  components: {
    Button,
    Dialog,
    Message,
    Card,
    Calendar,
    Dropdown,
    AutoComplete,
    Navbar,
    Menubar1,
  },
  data() {
    return {
      displayConfirmation: false,
      showError: false,
      errorMessage: "",
      value: null,
      seanceTypes: [
        { name: "Normal", code: 0 },
        { name: "Premiere", code: 1 },
        { name: "Marathon", code: 2 },
      ],
      seance: {
        id: null,
        date: null,
        seanceType: 0,
        movieId: 0,
        cinemaHallId: 0,
      },
      cinemas: null,
      cinemaHalls: null,
      movies: null,
      selectedCinema: null,
      filteredCinemas: null,
      selectedCinemaHall: null,
      filteredCinemaHalls: null,
      selectedMovie: null,
      filteredMovies: null,
    };
  },
  created() {
    const route = useRoute();
    const id = route.params.id;

    SeanceService.get(id)
      .then((response) => {
        this.seance = response.data;
        this.selectedCinemaHall = response.data.cinemaHall;
        this.selectedMovie = response.data.movie;
        this.selectedCinema = response.data.cinemaHall.cinema;
      })
      .catch((error) => {
        if (error.response.status === 404) {
          this.$router.replace({
            name: "NotFound",
            params: { err: error.response.data.Message },
          });
        }
      });

    CinemaService.getAll()
      .then((response) => {
        this.cinemas = response.data.items;
      })
      .catch((error) => {
        if (error.response.status === 404) {
          this.$router.replace({
            name: "NotFound",
            params: { err: error.response.data.Message },
          });
        }
      });

    MovieService.getAll(1, 10000000)
      .then((response) => {
        this.movies = response.data.items;
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
      this.$router.push({ name: "Seances" });
    },
    deleteSeance(id) {
      SeanceService.delete(id)
        .then((response) => {
          console.log(response.data);
          this.$router.replace({ name: "Seances" });
        })
        .catch((error) => {
          console.log(error.response.data);
        });
      this.displayConfirmation = false;
    },
    createSeance() {
      this.seance.movieId = this.selectedMovie.id;
      this.seance.cinemaHallId = this.selectedCinemaHall.id;

      SeanceService.create(this.seance)
        .then((response) => {
          console.log(response.data);
          this.$router.replace({ name: "Seances" });
        })
        .catch((error) => {
          console.log(error);
          this.errorMessage = error.response.data.Message;
          this.showError = true;
        });
    },
    updateSeance() {
      const request = {
        seanceId: this.seance.id,
        date: this.seance.date,
        seanceType: this.seance.seanceType,
        movieId: this.selectedMovie.id,
        cinemaHallId: this.selectedCinemaHall.id,
      };

      SeanceService.update(this.seance.id, request)
        .then((response) => {
          console.log(response.data);
          this.$router.replace({ name: "Seances" });
        })
        .catch((error) => {
          console.log(error);
          this.errorMessage = error.response.data.Message;
          this.showError = true;
        });
    },
    submit() {
      if (this.seance.id === null) {
        console.log("create");
        this.createSeance();
      } else {
        console.log("update");
        this.updateSeance();
      }
    },
    getCinemaHallsInCinema(cinemaId) {
      CinemaHallService.getInCinema(cinemaId)
        .then((response) => {
          this.cinemaHalls = response.data.items;
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
    searchCinema(event) {
      setTimeout(() => {
        if (!event.query.trim().length) {
          this.filteredCinemas = [...this.cinemas];
        } else {
          this.filteredCinemas = this.cinemas.filter((cinema) => {
            return cinema.name
              .toLowerCase()
              .startsWith(event.query.toLowerCase());
          });
        }
      }, 250);
    },
    searchCinemaHall(event) {
      this.getCinemaHallsInCinema(this.selectedCinema.id);
      setTimeout(() => {
        if (!event.query.trim().length) {
          this.filteredCinemaHalls = [...this.cinemaHalls];
        } else {
          this.filteredCinemaHalls = this.cinemaHalls.filter((cinemaHall) => {
            return cinemaHall.name
              .toLowerCase()
              .startsWith(event.query.toLowerCase());
          });
        }
      }, 250);
    },
    searchMovie(event) {
      setTimeout(() => {
        if (!event.query.trim().length) {
          this.filteredMovies = [...this.movies];
        } else {
          this.filteredMovies = this.movies.filter((movie) => {
            return movie.title
              .toLowerCase()
              .startsWith(event.query.toLowerCase());
          });
        }
      }, 250);
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