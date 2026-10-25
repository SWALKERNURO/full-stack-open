def add_setting(settings, setting):
    key, value = (item.lower() for item in setting)
    if key in settings:
        return f"Setting '{key}' already exists! Cannot add a new setting with this name."
    settings[key] = value
    return f"Setting '{key}' added with value '{value}' successfully!"


def update_setting(settings, setting):
    key, value = (item.lower() for item in setting)
    if key not in settings:
        return f"Setting '{key}' does not exist! Cannot update a non-existing setting."
    settings[key] = value
    return f"Setting '{key}' updated to '{value}' successfully!"


def delete_setting(settings, key):
    key = key.lower()
    if key not in settings:
        return 'Setting not found!'
    del settings[key]
    return f"Setting '{key}' deleted successfully!"


def view_settings(settings):
    if not settings:
        return 'No settings available.'
    lines = [f'{key.capitalize()}: {value}' for key, value in settings.items()]
    return 'Current User Settings:\n' + '\n'.join(lines) + '\n'


test_settings = {'theme': 'dark', 'language': 'english', 'notifications': 'enabled'}
