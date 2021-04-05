import React, { Component } from 'react';
import taskService from '../services/taskService';

import { Button, Input, InputLabel, FormControl, Grid, Paper, Box } from '@material-ui/core';
class CreateTask extends Component{
    constructor(props) {
        super(props);
        this.state = {
        file: "",
        text: "",
        }
        this.onFormSubmit = this.onFormSubmit.bind(this);
        this.onChange = this.onChange.bind(this);
    }
  

     async onFormSubmit(e) {
        e.preventDefault()
        const { file, text } = this.state;
        const stcheck = await taskService.createTask(file, text);
        this.props.handleStateChange();


    }

    onChange(e){
        switch (e.target.type) {
            case 'file':
                this.setState({ file: e.target.files[0] });
                break;
            case 'text':
                this.setState({ text: e.target.value });
                break;
        }

    }

    render() {
        return (
            <div style={{ width: '50%'}}>

                <Paper style={{ padding: '4em', display: 'flex', 'flexDirection': 'column', height: '100%', 'justifyContent': 'end', 'alignItems':'flex-start' }}>
                    <h1>Task App</h1>
                    <Box style={{ 'width':'90%'}}>
                    <form onSubmit={this.props.action} >
                        <Grid container alignItems="center" spacing={2}>
                            <Grid item xs={12}>
                                <FormControl>
                                    <InputLabel htmlFor="task-input">Type in a new task</InputLabel>
                                    <Input id="task-input" type="text" name="text" value={this.state.text} onChange={this.onChange} required />
                                </FormControl>
                            </Grid>
                            <Grid item xs={12}>
                                <label htmlFor="btn-upload">
                                    <input
                                        id="btn-upload"
                                        name="btn-upload"
                                        style={{ display: 'none' }}
                                        type="file"
                                        accept="image/*"
                                        onChange={this.onChange}
                                        required />
                                    <Button
                                        className="btn-choose"
                                        variant="outlined"
                                        component="span" >
                                        Choose Image
                                    </Button>
                                </label>
                            </Grid>

                                <Button className="btn-upload" color="primary" variant="contained" component="span" type="submit" onClick={this.onFormSubmit} style={{ 'alignItems': 'flex-end', 'justifyContent': 'center', 'alignSelf': 'center' }}> Add Task</Button>
                        </Grid>
                        </form>
                        </Box>
                    </Paper>
         
              </div>
            )
    }
}

export default CreateTask;