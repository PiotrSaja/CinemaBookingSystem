<template>
<Card class="p-4">
    <template #title>
         <Message severity="error" v-if="showError" :closable="false">{{errorMessage}}</Message>
        Import Movie
    
    </template>
    <template #content>
        <span>To add a movie, please enter imdb id from <a href="https://www.imdb.com/">imdb.com</a></span>
        <span class="p-float-label mt-4">
            <InputText id="imdbId" type="text" v-model="request.imdbId" />
            <label for="imdbId" >Imdb Id</label>
        </span>
    </template>
    <template #footer>
        <Button icon="pi pi-check" label="Submit" @click="createMovie()"/>
        <Button icon="pi pi-times" label="Cancel" class="p-button-secondary" style="margin-left: .5em" @click="goBack()"/>
    </template>
</Card>
</template>

<script>
import MovieService from '@/services/movie-service';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Card from 'primevue/card';
import Message from 'primevue/message';
export default {
    name: 'NewMovieView',
    components: {
        Button,
        InputText,
        Card,
        Message
    },
    data() {
        return {
            request: {
                imdbId : ''
            },
            showError: false,
            errorMessage: ''
        }
    },
    methods: {
        async createMovie () {
            await MovieService.create(this.request).then((response) => {
                console.log(response.data)
                this.$router.replace({name: 'Movies'})
            }).catch(error => {
                console.log(error)
                this.errorMessage = error.response.data.Message;
                this.showError = true;
            })
        },
        goBack () {
            this.$router.replace({name: 'Movies'})
        }
    }
}
</script>

<style lang="scss"  scoped>
</style>