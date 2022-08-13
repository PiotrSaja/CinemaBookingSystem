<template>
    <card class="mt-3">
        <template #title>
            Update cinema hall
        </template>
        <template #content>
            <div class="ml-2">
                <Message severity="error" v-if="showError" :closable="false">{{errorMessage}}</Message>
                    <div class="card">
                        <h5>General information</h5>
                        <div class="field grid">
                            <label for="cinemaName" class="col-12 mb-2 md:col-2 md:mb-0">Name</label>
                            <div class="col-12 md:col-10">
                                <input id="cinemaName" type="text" class="text-base text-color surface-overlay p-2 border-1 border-solid surface-border border-round appearance-none outline-none focus:border-primary w-full" v-model="cinemaHall.name">
                            </div>
                        </div>
                        <h5>Seats information</h5>
                        <div class="field grid">
                            <label for="street" class="col-12 mb-2 md:col-2 md:mb-0">Number of rows</label>
                            <div class="col-12 md:col-10">
                                <input id="street" type="number" class="text-base text-color surface-overlay p-2 border-1 border-solid surface-border border-round appearance-none outline-none focus:border-primary w-full" v-model="cinemaHall.numberOfRows">
                            </div>
                        </div>
                        <div class="field grid">
                            <label for="city" class="col-12 mb-2 md:col-2 md:mb-0">Number of columns</label>
                            <div class="col-12 md:col-10">
                                <input id="city" type="number" class="text-base text-color surface-overlay p-2 border-1 border-solid surface-border border-round appearance-none outline-none focus:border-primary w-full" v-model="cinemaHall.numberOfColumns">
                            </div>
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
                                <Button label="Yes" icon="pi pi-check" @click="deleteCinemaHall(cinemaHall.id)" class="p-button-text" autofocus />
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
import { useRoute } from 'vue-router';
import Card from 'primevue/card';
export default {
    name: 'CinemaHallView',
    components: {
        Button,
        Dialog,
        Message,
        Card
    },
    data() {
        return {
            displayConfirmation: false,
            displayAddHall: false,
            cinemaId: 0,
            cinemaHall: {
                id: null,
                name: '',
                totalSeats: null,
                numberOfRows: null,
                numberOfColumns: null,
                cinemaId: null,
            },
            cinemaHallForm: {
                id: null,
                name: '',
                totalSeats: null,
                numberOfRows: null,
                numberOfColumns: null,
                cinemaId: null,
            },
            showError: false,
            errorMessage: ''
        }
    },
    created () {
        const route = useRoute(); 
        const id = route.params.id;
        this.cinemaId = route.params.cinemaId;
        this.cinemaHall.cinemaId = route.params.cinemaId;
        CinemaHallService.get(id).then((response) => {
            this.cinemaHall = response.data
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
            this.$router.push({name: 'CinemaHalls', params: {id: this.cinemaId}})
        },
        deleteCinemaHall(id) {
            CinemaHallService.delete(id).then((response) => {
                    console.log(response.data)
                    this.$router.replace({name: 'CinemaHalls', params: {id: this.cinemaId}})
                }).catch((error) => {
                    console.log(error.response.data)
            })
            this.displayConfirmation = false;
        },
        createCinemaHall () {
            this.cinemaHall.totalSeats = this.cinemaHall.numberOfRows * this.cinemaHall.numberOfColumns;

            CinemaHallService.create(this.cinemaHall).then((response) => {
                console.log(response.data)
                this.$router.replace({name: 'CinemaHalls', params: {id: this.cinemaId}})
            }).catch(error => {
                console.log(error)
                this.errorMessage = error.response.data.Message;
                this.showError = true;
            })
        },
        updateCinemaHall () {
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
                this.updateCinemaHall();
            }else{
                console.log
                this.createCinemaHall();
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