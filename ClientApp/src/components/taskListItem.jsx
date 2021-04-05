import React, { Component } from "react";
import { apiUrl } from '../../src/config.json';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import ListItemAvatar from '@material-ui/core/ListItemAvatar';


function TaskListItem(props) {
    const { id, text, fileExtension } = props.task;

        return (
        <div>
                <ListItem style={{ 'flexDirection': 'row-reverse'}} key={props.task.id} button>
                            <ListItemAvatar>
                        <img src={`${apiUrl}/image/${id}${fileExtension}`} width="40" />
                            </ListItemAvatar>
                    <ListItemText id={id} primary={text} />
                      
                        </ListItem>
                  

        </div>
    );
    };


export default TaskListItem;