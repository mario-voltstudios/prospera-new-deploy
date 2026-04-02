# Project Overview

This is a Supabase project for an insurance management application named "Prospera". The project is configured to use a PostgreSQL database, and it includes a database schema with tables for managing insurance agents, policies, receipts, and commissions.

The project uses Supabase for its backend, including database hosting, authentication, and APIs. The database schema is managed through migration files located in the `supabase/migrations` directory.

## Building and Running

### Local Development

To run the project locally, you will need to have the Supabase CLI installed.

1.  **Start the Supabase services:**

    ```bash
    supabase start
    ```

2.  **Apply the database migrations:**

    ```bash
    supabase db reset
    ```

### Testing

There are no explicit tests included in this project.

TODO: Add instructions for running tests once a testing framework is in place.

## Development Conventions

### Database

The database schema is defined in the SQL migration files in the `supabase/migrations` directory. When making changes to the database schema, create a new migration file using the Supabase CLI:

```bash
supabase migration new <migration_name>
```

### Coding Style

The project does not currently have a defined coding style.

TODO: Define and document a coding style for the project.

### Commit Messages

Commit messages should follow the Conventional Commits specification. This creates a more readable history and makes it easier to automate things like generating changelogs.

Each commit message consists of a **header**, a **body** and a **footer**.

```
<type>(<scope>): <subject>
<BLANK LINE>
<body>
<BLANK LINE>
<footer>
```

**Type**: Must be one of the following:

*   **feat**: A new feature
*   **fix**: A bug fix
*   **docs**: Documentation only changes
*   **style**: Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)
*   **refactor**: A code change that neither fixes a bug nor adds a feature
*   **perf**: A code change that improves performance
*   **test**: Adding missing tests or correcting existing tests
*   **build**: Changes that affect the build system or external dependencies (example scopes: gulp, broccoli, npm)
*   **ci**: Changes to our CI configuration files and scripts (example scopes: Travis, Circle, BrowserStack, SauceLabs)
*   **chore**: Other changes that don't modify `src` or `test` files
*   **revert**: Reverts a previous commit

**Scope**: The scope could be anything specifying place of the commit change. For example `db`, `api`, `auth`, etc.

**Subject**: The subject contains succinct description of the change.

**Example:**

```
feat(auth): add password reset functionality
```
