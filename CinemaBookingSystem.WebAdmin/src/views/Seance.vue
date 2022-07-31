<template>
    <card class="mt-3">
        <template #title>
            Edit seance
        </template>
        <template #content>
            <div class="ml-2">
                <Message severity="error" v-if="showError" :closable="false">{{errorMessage}}</Message>
                    <div class="card">
                        <h5>General information</h5>
                        <label for="seanceDate" class="col-12 mb-2 md:col-2 md:mb-0">Seance date</label>
                        <div class="col-12 md:col-10">
                            <Calendar id="seanceDate" v-model="seance.date" :showTime="true" :showSeconds="false" />
                        </div>
                        <label for="seanceType" class="col-12 mb-2 md:col-2 md:mb-0">Seance type</label>
                        <div class="col-12 md:col-10">
                            <Dropdown v-model="seance.seanceType" :options="seanceTypes" optionLabel="name" placeholder="Select a seance type" required/>
                        </div>
                        <h5>Hall information</h5>
                        <label for="cinema" class="col-12 mb-2 md:col-2 md:mb-0">Cinema</label>
                        <div class="col-12 md:col-10">
                            <AutoComplete v-model="selectedCountry" :suggestions="filteredCountriesBasic" @complete="searchCountry($event)" field="name" />
                        </div>
                        <label for="hall" class="col-12 mb-2 md:col-2 md:mb-0">Cinema Hall</label>
                        <div class="col-12 md:col-10">
                            <AutoComplete v-model="selectedCountry" :suggestions="filteredCountriesBasic" @complete="searchCountry($event)" field="name" />
                        </div>
                       
                        <h5>Movie information</h5>
                        <label for="movie" class="col-12 mb-2 md:col-2 md:mb-0">Movie</label>
                        <div class="col-12 md:col-10">
                            <AutoComplete v-model="selectedCountry" :suggestions="filteredCountriesBasic" @complete="searchCountry($event)" field="title" />
                        </div>
                    </div>
                    <div class="text-right row mt-5">
                        <Button label="Submit" icon="pi pi-check" iconPos="left" class="p-button-success mr-2"  @click="submit()"/>
                        <Button label="Cancel" icon="pi pi-times" iconPos="left" class="p-button-info mr-2" @click="goBack()" />
                        <Button label="Delete" icon="pi pi-trash" @click="openConfirmation" class="p-button-danger" />
                        <Dialog header="Delete" v-model:visible="displayConfirmation" :style="{width: '350px'}" :modal="true">
                            <div class="confirmation-content">
                                <i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
                                <span>Are you sure you want to delete ?</span>
                            </div>
                            <template #footer>
                                <Button label="No" icon="pi pi-times" @click="closeConfirmation" class="p-button-text"/>
                                <Button label="Yes" icon="pi pi-check" @click="deleteSeance(seance.id)" class="p-button-text" autofocus />
                            </template>
                        </Dialog>
                    </div>
                </div>
        </template>
    </card>
</template>

<script>
import Message from 'primevue/message';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import CinemaHallService from '@/services/cinema-hall-service';
import CinemaService from '@/services/cinema-service';
import SeanceService from '@/services/seance-service';
import { useRoute } from 'vue-router';
import Card from 'primevue/card';
import Calendar from 'primevue/calendar';
import Dropdown from 'primevue/dropdown';
import AutoComplete from 'primevue/autocomplete';
export default {
    name: 'SeanceView',
    components: {
        Button,
        Dialog,
        Message,
        Card,
        Calendar,
        Dropdown,
        AutoComplete
    },
    data() {
        return {
            displayConfirmation: false,
            showError: false,
            errorMessage: '',
            value: null,
            seanceTypes: [
                {name: 'Normal', code: 0},
                {name: 'Premiere', code: 1},
                {name: 'Marathon', code: 2}
            ],
            seance: {
                id: null,
                date: null,
                seanceType: 0,
                movieId: 0,
                cinemaHallId: 0
            },
            cinemas: [],
            cinemaHalls: [],
            selectedCinemaId: null,
        }
    },
    created () {
        const route = useRoute(); 
        const id = route.params.id;
        SeanceService.get(id).then((response) => {
            this.seance = response.data
            }).catch(error => {
            if (error.response.status === 404) {
                this.$router.replace({name: 'NotFound', params: {err: error.response.data.Message}})
            }
        })

        CinemaService.getAll().then((response) => {
            this.cinemas = response.data
            }).catch(error => {
            if (error.response.status === 404) {
                this.$router.replace({name: 'NotFound', params: {err: error.response.data.Message}})
            }
        })

        CinemaHallService.getInCinema(this.selectedCinemaId).then((response) => {
            this.cinemaHalls = response.data
            }).catch(error => {
            if (error.response.status === 404) {
                this.$router.replace({name: 'NotFound', params: {err: error.response.data.Message}})
            }
        })
    },
    methods: {
        openConfirmation() {
            this.displayConfirmation = true;
        },
        closeConfirmation() {
            this.displayConfirmation = false;
        },
        goBack() {
            this.$router.push({name: 'Seances'})
        },
        deleteSeance(id) {
            SeanceService.delete(id).then((response) => {
                    console.log(response.data)
                    this.$router.replace({name: 'Seances'})
                }).catch((error) => {
                    console.log(error.response.data)
            })
            this.displayConfirmation = false;
        },
        createSeance () {
            SeanceService.create(this.seance).then((response) => {
                console.log(response.data)
                this.$router.replace({name: 'Seances'})
            }).catch(error => {
                console.log(error)
                this.errorMessage = error.response.data.Message;
                this.showError = true;
            })
        },
        updateSeance () {
            this.cinemaHallForm.id = this.cinemaHall.id;
            this.cinemaHallForm.name = this.cinemaHall.name;
            this.cinemaHallForm.totalSeats = this.cinemaHall.numberOfRows * this.cinemaHall.numberOfColumns;
            this.cinemaHallForm.numberOfRows = this.cinemaHall.numberOfRows;
            this.cinemaHallForm.numberOfColumns = this.cinemaHall.numberOfColumns;
            this.cinemaHallForm.cinemaId = this.cinemaHall.cinema.id;

            CinemaHallService.update(this.cinemaHallForm.id, this.cinemaHallForm).then((response) => {
                console.log(response.data)
                this.$router.replace({name: 'CinemaHalls', params: {id: this.cinemaId}})
            }).catch(error => {
                console.log(error)
                this.errorMessage = error.response.data.Message;
                this.showError = true;
            })
        },
        submit () {
            if(this.cinemaHall.id !== null){
                this.createSeance();
            }else{
                this.updateSeance();
            }
        }
    }
}
</script>

<style scoped lang="scss">
a {
    text-decoration: none;
    color: black;
}
.field * {
    display: block;
}
</style>