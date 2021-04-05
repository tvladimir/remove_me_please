import React, { Component } from 'react';
import { flexbox , Box } from '@material-ui/core';
import CreateTask from './createTask';
import TaskList from './taskList';
import taskService from '../services/taskService';


export class Home extends Component {
  static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = {
            tasks: [],
        }
        this.handleStateChange = this.handleStateChange.bind(this);
        this.handleServerUpdate = this.handleServerUpdate.bind(this);
  
    }
    async handleStateChange() {
        try {
            const { data } = await taskService.getAllTasks();
            if (data.length > 0) {
                console.log(data)
                this.setState({ tasks: data });
            }
        } catch (err) {
            if (err.response && err.response.status === 401) {
                this.setState({ errors: { cards: "No tasks to show you my" } });
            }
        }
    }
 

    async componentDidMount() {
        try {
            const { data } = await taskService.getAllTasks();
            if (data.length > 0) this.setState({ tasks: data });
        } catch (err) {
            if (err.response && err.response.status === 401) {
                this.setState({ errors: { cards: "No tasks to show you my" } });
            }
        }

    }

    handleServerUpdate(alltasks) {
        this.setState({tasks: alltasks})
    }

  render () {
      return (
          <Box style={{display: 'flex', 'flexDirection':'row', height:'50vh'}}>
            <CreateTask handleStateChange={this.handleStateChange} />
            <TaskList handleServerUpdate={this.handleServerUpdate} tasks={this.state.tasks}/>
      </Box>
    );
  }
}
